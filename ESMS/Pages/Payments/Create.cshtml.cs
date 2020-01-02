﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ESMS
{
    [Authorize(Policy = "Payment:Create")]
    public class CreateModel : BaseModel
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public IConfiguration configuration { get; set; }
        public CreateModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHubContext<NotificationHub> _hubContext) :base(signInManager, userManager) {
            this.configuration = configuration;
            this._hubContext = _hubContext;
        }

        public void OnGet()
        {
            employees = dbContext.AspNetUsers.Where(u=>u.AspNetUserRoles.FirstOrDefault().Role.Name != "Administrator" && u.EmployeeStatus == 1).Select(U => new Employee
            {
                FirstName = U.FirstName,
                LastName = U.LastName,
                salary = (decimal)U.Salary,
                Role = U.AspNetUserRoles.FirstOrDefault().Role.Name,
                Iban = U.IbanCode
            }).ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listOfEmployee = dbContext.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().Role.Name != "Administrator" && u.EmployeeStatus == 1).Select(U => new SalaryPayment
                    {
                        Ammount = (decimal)U.Salary,
                        Iban = U.IbanCode
                    }).ToList();

                    using (HttpClient client = new HttpClient())
                    {
                        StringContent bankContent = new StringContent(JsonConvert.SerializeObject(listOfEmployee), encoding: Encoding.UTF8, "application/json");
                        var request = await client.PostAsync(configuration.GetSection("AppSettings").GetSection("BankApiLink").Value, bankContent);
                        var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
                        if (response.nError == 0)
                        {
                            //Insertimi i pagesave ne sistem
                            List<Payments> payments = dbContext.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().Role.Name != "Administrator" && u.EmployeeStatus == 1).Select(U => new Payments
                            {
                                Salary = (decimal)U.Salary,
                                DtInserted = DateTime.Now,
                                Month = Input.month,
                                UserId = U.Id,
                                VcInserted = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            }).ToList();
                            dbContext.Payments.AddRange(payments);

                            string month = dbContext.Month.Where(M => M.Id == Input.month).Select(s => s.MonthSq).FirstOrDefault();
                            var notifications = dbContext.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().Role.Name != "Administrator" && u.EmployeeStatus == 1).Select(U => new Notifications
                            {
                                Title = "Pagat për muajin " + month,
                                DtInserted = DateTime.Now,
                                VcIcon = "zmdi zmdi-money",
                                VcInsertedUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                                VcUser = U.Id,
                                VcText = "Ka dalë paga për muajin " + month + ", në vlerë prej " + string.Format("{0:C}", U.Salary) + " euro."
                            }).ToList();
                            dbContext.Notifications.AddRange(notifications);
                            await dbContext.SaveChangesAsync();

                            foreach (var notification in notifications)
                                await _hubContext.Clients.All.SendAsync(notification.VcUser, notification.VcText, notification.Title, "info", "/");
                            TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
                            return RedirectToPage("List");
                        }
                    }
                }
                else
                    error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            catch (Exception ex)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return Page();
        }

        public Error error { get; set; }

        [BindProperty]
        public InputClass Input { get; set; }

        public class InputClass
        {
            [Range(1,12)]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "muaji", ResourceType = typeof(Resource))]
            public int month { get; set; }
        }

        public List<Employee> employees { get; set; }

        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public decimal salary { get; set; }
            public string Role { get; set; }
            public string Iban { get; set; }
        }

        public class SalaryPayment
        {
            public string Iban { get; set; }
            public decimal Ammount { get; set; }
        }

        public class ApiResponse {
            public int nError { get; set; }
            public string errorDescription { get; set; }
        }
    }
}
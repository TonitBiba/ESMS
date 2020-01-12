using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
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
            employees = dbContext.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().Role.Name != "Administrator" && u.EmployeeStatus == 1)
                .Select(S => new Employee
            {
                FirstName = S.FirstName,
                LastName = S.LastName,
                IdCard = S.PersonalNumber,
                Taxgroup = GetTaxGroupNameFromID(S.TaxGroupId),
                Salaryforcalculation = (decimal)S.Salary,
                EmployeePension = 0,
                EmployerPension = 0,
                TaxableIncome = 0,
                WithholdingTax = 0,
                NetWage = 0,
                PositionName = S.JobTitle,
                userId = S.Id
            }).ToList();
        }

        public async Task<JsonResult> OnPostExecute(InputClass I)
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
                            List<Payments> payments = new List<Payments>();
                            foreach (var user in I.deductions)
                            {
                                var UserDetails = dbContext.AspNetUsers.Where(U => U.Id == user.id).FirstOrDefault();
                                var deduction =  Convert.ToDecimal(user.money, CultureInfo.InvariantCulture);
                                payments.Add(new Payments
                                {
                                    Deduction = deduction,
                                    Month = I.month,
                                    DtInserted = DateTime.Now,
                                    Salary = (decimal)UserDetails.Salary,
                                    UserId = user.id,
                                    EmployeePension = Convert.ToDecimal(CalculateSalaryGeneral.CalculateEmployeePension((decimal)(UserDetails.Salary), UserDetails.TaxGroupId)),
                                    EmployerPension = Convert.ToDecimal(CalculateSalaryGeneral.CalculateEmployeePension((decimal)(UserDetails.Salary), UserDetails.TaxGroupId)),
                                    NetWage = Convert.ToDecimal(CalculateSalaryGeneral.CalculateNetWage((decimal)(UserDetails.Salary), UserDetails.TaxGroupId)),
                                    SalaryAfterDeduction = CalculateSalaryWithDeduction.CalculateSalaryWithDed(UserDetails.Salary, deduction),
                                    TaxableIncome = Convert.ToDecimal(CalculateSalaryGeneral.CalculateTaxableIncome((decimal)(UserDetails.Salary), UserDetails.TaxGroupId)),
                                    VcInserted = User.FindFirstValue(ClaimTypes.NameIdentifier),
                                    WithholdingTax = Convert.ToDecimal(CalculateSalaryGeneral.CalculateWithHoldingTax((decimal)(UserDetails.Salary), UserDetails.TaxGroupId))
                                });
                            }
                            dbContext.Payments.AddRange(payments);

                            string month = dbContext.Month.Where(M => M.Id == Input.month).Select(s => s.MonthSq).FirstOrDefault();
                            var notifications = dbContext.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().Role.Name != "Administrator" && u.EmployeeStatus == 1).Select(U => new Notifications
                            {
                                Title = "Pagat për muajin " + month,
                                DtInserted = DateTime.Now,
                                VcIcon = "zmdi zmdi-money",
                                VcInsertedUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                                VcUser = U.Id,
                                VcText = "Ka dalë paga për muajin " + month + ", në vlerë prej " + string.Format(new CultureInfo("en-US"), "{0:C}", U.Salary).Substring(1) + " euro."
                            }).ToList();
                            dbContext.Notifications.AddRange(notifications);
                            await dbContext.SaveChangesAsync();

                            foreach (var notification in notifications)
                                await _hubContext.Clients.All.SendAsync(notification.VcUser, notification.VcText, notification.Title, "info", "/");
                            error = new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses };
                        }
                        else
                            error = new Error { nError = 4, ErrorDescription = Resource.apiError };
                    }
                }
                else
                    error = new Error { nError = 4, ErrorDescription = Resource.invalidData };
            }
            catch(HttpRequestException httEx)
            {
                SaveLog(httEx, HttpContext);
                error = new Error { nError = 4, ErrorDescription = Resource.apiError };
            }
            catch (Exception ex)
            {
                SaveLog(ex, HttpContext);
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return new JsonResult(error);
        }

        public IActionResult OnGetAccountantList(int m, int f)
        {
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("reportuser", "Esms2019.");
                reportBytes = client.DownloadData("http://localhost/ReportServer/Pages/ReportViewer.aspx?%2fESMSReports%2fAccountantReport&rs:Command=Render&rs:Format=Excel&month=" + m+ "&monthName="+dbContext.Month.Where(t=>t.Id == m).FirstOrDefault().MonthSq);
            }

            return File(reportBytes, "application/Excel".ToLower(), Resource.accountantList+" " + DateTime.Now.ToShortDateString() + ".xls");
        }

        public static string GetTaxGroupNameFromID(int taxgroupId)
        {
            return taxgroupId switch
            {
                1 => Resource.pensionaRegular,
                2 => Resource.noPensionRegular,
                3 => Resource.nonRegularPension,
                _ => Resource.nonRegularNoPension,
            };
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
            public List<Deduction> deductions { get; set; }
        }

        public List<Employee> employees { get; set; }

        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string IdCard { get; set; }
            public string Taxgroup { get; set; }
            public decimal Salaryforcalculation { get; set; }
            public decimal EmployeePension { get; set; }
            public decimal EmployerPension { get; set; }
            public decimal TaxableIncome { get; set; }
            public decimal WithholdingTax { get; set; }
            public decimal NetWage { get; set; }
            public string PositionName { get; set; }
            public string userId { get; set; }
        }

        public class SalaryPayment
        {
            public string Iban { get; set; }
            public decimal Ammount { get; set; }
        }

        public class Deduction
        {
            public string id { get; set; }
            public string money { get; set; }
        }

        public class ApiResponse {
            public int nError { get; set; }
            public string errorDescription { get; set; }
        }
    }
}
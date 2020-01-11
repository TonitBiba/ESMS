using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Employees
{
    [Authorize(Policy = "Employee:List")]
    public class ListModel : BaseModel
    {
        public ListModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) {}

        public void OnGet()
        {
            string userGroupId = dbContext.AspNetUserRoles.Where(UR => UR.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().RoleId;
            string[] getGroups = new string[1];
            if (User.IsInRole("Menagjer_IT"))
            {
                getGroups = new string[] { "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e" };
            }
            else if(User.IsInRole("Menagjer_Financa"))
            {
                getGroups = new string[] { "dbc05ab9-f41f-493f-b3e6-689d14e88dda" };
            }else if(User.IsInRole("Administrator"))
            {
                getGroups = new string[] { "dbc05ab9-f41f-493f-b3e6-689d14e88dda", "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e", "423a5ce2-3024-47d1-b486-4dcd3951871b", "be007199-39b1-4557-b10f-cc4e6dc47b49", "58cfa2e9-9eeb-4fcd-b87c-6d0b676fb066" };
            }else if (User.IsInRole("Burimet_Njerzore"))
            {
                getGroups = new string[] { "dbc05ab9-f41f-493f-b3e6-689d14e88dda", "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e", "423a5ce2-3024-47d1-b486-4dcd3951871b", "be007199-39b1-4557-b10f-cc4e6dc47b49", "58cfa2e9-9eeb-4fcd-b87c-6d0b676fb066" };
            }

            employees = dbContext.AspNetUsers.Where(U => getGroups.Contains(U.AspNetUserRoles.FirstOrDefault().RoleId) && U.EmployeeStatus == 1).Select(A => new List { 
                 FirstName = A.FirstName,
                 LastName = A.LastName,
                 Birthdate = A.BirthDate,
                 Email = A.Email,
                 DtFrom = A.DtFrom,
                 DtTo = A.DtTo,
                 Gender = A.Gender ==1?"Mashkull":"Femer",
                 PhoneNumber = A.PhoneNumber,
                 Salary = A.Salary,
                 statusEmployee = A.EmployeeStatus == 0?"Pasiv":"Aktiv",
                 UserId = A.Id,
                 Role = A.AspNetUserRoles.FirstOrDefault().Role.Name
            }).ToList();

            error = TempData.Get<Error>("error");
        }

        public List<List> employees { get; set; }

        public IActionResult OnGetUsers(int f)
        {
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                string userGroupId = dbContext.AspNetUserRoles.Where(UR => UR.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().RoleId;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("reportuser","Esms2019.");
                reportBytes = client.DownloadData("http://tonit/ReportServer/Pages/ReportViewer.aspx?%2fESMSReports%2fUsers&groupID=" + userGroupId + "&statusi=1&gender=0&rs:Format=" + getFormatReport(f));
            }
            return File(reportBytes, "application/"+ getFormatReport(f).ToLower(), f!=1 ? "Përdoruesit " +DateTime.Now.ToShortDateString()+ getExtension(f):"");
        }

        public JsonResult OnPostDelete(string UEnc)
        {
            Error error = new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses };
            try
            {
                string userId = Confidenciality.Decrypt<string>(UEnc);
                dbContext.AspNetUsers.Where(U => U.Id == userId).FirstOrDefault().EmployeeStatus = 0;
                dbContext.SaveChanges();
            }catch(Exception ex)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return new JsonResult(error);
        }

        public Error error { get; set; }

        public class List
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public DateTime Birthdate { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime DtFrom { get; set; }
            public DateTime DtTo { get; set; }
            public float Salary { get; set; }
            public string statusEmployee { get; set; }
            public string UserId { get; set; }
        }
    }
}
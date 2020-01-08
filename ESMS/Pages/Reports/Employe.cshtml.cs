using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Reports
{
    public class EmployeModel : BaseModel
    {
        public EmployeModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }

        public void OnGet()
        {

        }

        public async Task<PartialViewResult> OnPostSearch(int? gender, int? status)
        {
            var users = dbContext.AspNetUsers.Where(t=>t.EmployeeStatus == (status==null ? t.EmployeeStatus : status)
                && t.Gender == (gender == null ? t.Gender : gender)
                && t.AspNetUserRoles.FirstOrDefault().RoleId != "2a13875f-53af-45a5-b240-48a90ff993a5"
            ).Select(A => new UserModel
            {
                FirstName = A.FirstName,
                LastName = A.LastName,
                Birthdate = A.BirthDate,
                Email = A.Email,
                DtFrom = A.DtFrom,
                DtTo = A.DtTo,
                Gender = A.Gender == 1 ? "Mashkull" : "Femer",
                PhoneNumber = A.PhoneNumber,
                Salary = A.Salary,
                statusEmployee = A.EmployeeStatus == 0 ? "Pasiv" : "Aktiv",
                UserId = A.Id,
                Role = A.AspNetUserRoles.FirstOrDefault().Role.Name
            }).ToList();
            TempData["model"] = users;
            return Partial("EmployeList");
        }

        public IActionResult OnGetUsers(int f, int? status, int gender)
        {
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                string userGroupId = dbContext.AspNetUserRoles.Where(UR => UR.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().RoleId;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("reportuser", "Esms2019.");
                reportBytes = client.DownloadData("http://tonit/ReportServer/Pages/ReportViewer.aspx?%2fESMSReports%2fUsers&groupID=" + userGroupId + "&statusi=" + status + "&gender=" + gender + "&rs:Format=" + getFormatReport(f));
            }

            return File(reportBytes, "application/" + getFormatReport(f).ToLower(), f != 1 ? "Përdoruesit " + DateTime.Now.ToShortDateString() + getExtension(f) : "");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "statusi", ResourceType = typeof(Resource))]
            public bool status { get; set; }

            [Display(Name = "gjinia", ResourceType = typeof(Resource))]
            public int gender { get; set; }
        }

        public class UserModel
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
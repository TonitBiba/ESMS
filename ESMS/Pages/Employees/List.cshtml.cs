using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Data.Model;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Employees
{
    public class ListModel : BaseModel
    {
        public void OnGet()
        {
            string userGroupId = dbContext.AspNetUserRoles.Where(UR => UR.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().RoleId;

            employees = dbContext.AspNetUsers.Select(A => new List { 
                 FirstName = A.FirstName,
                 LastName = A.LastName,
                 Birthdate = A.BirthDate,
                 Email = A.Email,
                 EmployementDate = A.EmploymentDate,
                 Gender = A.Gender ==1?"Mashkull":"Femer",
                 PhoneNumber = A.PhoneNumber,
                 Salary = A.Salary,
                 statusEmployee = A.EmployeeStatus == 0?"Pasiv":"Aktiv",
                 UserId = A.Id,
                 Role = A.AspNetUserRoles.FirstOrDefault().Role.Name
            }).ToList();
        }

        public List<List> employees { get; set; }

        public IActionResult OnGetUsers(int f)
        {
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                string userGroupId = dbContext.AspNetUserRoles.Where(UR => UR.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().RoleId;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("reportuser","Esms2019.");
                reportBytes = client.DownloadData("http://tonit/ReportServer/Pages/ReportViewer.aspx?%2fESMSReports%2fUsers&groupID="+ userGroupId + "&rs:Format=" + getFormatReport(f));
            }

            return File(reportBytes, "application/"+ getFormatReport(f).ToLower(), f!=1 ? "Përdoruesit " +DateTime.Now.ToShortDateString()+ getExtension(f):"");
        }

        public class List
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public DateTime Birthdate { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime EmployementDate { get; set; }
            public float Salary { get; set; }
            public string statusEmployee { get; set; }
            public string UserId { get; set; }
        }
    }
}
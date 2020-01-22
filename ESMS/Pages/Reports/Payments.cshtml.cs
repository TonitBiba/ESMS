using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Reports
{
    public class PaymentsModel : BaseModel
    {
        public PaymentsModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }

        public void OnGet()
        {

        }

        public async Task<PartialViewResult> OnPostSearch(string dtFrom, string dtTo, string userId)
        {
            DateTime startDate = DateTime.ParseExact(dtFrom, "dd-MM-yyyy", null);
            DateTime endDate = DateTime.ParseExact(dtTo, "dd-MM-yyyy", null);

            var payments = dbContext.Payments.Where(S=>S.DtInserted>= startDate && S.DtInserted<= endDate 
                            && S.UserId == (userId == null ? S.UserId : userId)).Select(S => new Payments
            {
                Month = CultureInfo.CurrentCulture.Name=="en-US" ? S.MonthNavigation.MonthEn : S.MonthNavigation.MonthSq,
                ExecutionDate = S.DtInserted,
                FirstName = S.User.FirstName,
                LastName = S.User.LastName,
                Salary = S.Salary.ToString()
            }).ToList();
            TempData["model"] = payments;
            return Partial("PaymentsList");
        }

        public IActionResult OnGetReport(int f, string dtFrom, string dtTo, string userId)
        {
            DateTime startDate = DateTime.ParseExact(dtFrom, "dd-MM-yyyy", null);
            DateTime endDate = DateTime.ParseExact(dtTo, "dd-MM-yyyy", null);
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("username", "strongpassword");
                reportBytes = client.DownloadData("httpReportServerReportViewer.aspx?%2fESMSReports%2fSalariesPaid&rs:Command=Render&userId=" + userId + "&dtFrom="+ startDate.ToString("yyyy-MM-dd")+ "&dtTo="+ endDate.ToString("yyyy-MM-dd")+"&rs:Format=" + getFormatReport(f));
            }

            return File(reportBytes, "application/" + getFormatReport(f).ToLower(), f != 1 ? "Pagat " + DateTime.Now.ToShortDateString() + getExtension(f) : "");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "data", ResourceType = typeof(Resource))]
            public string dtFrom { get; set; }
            public string dtTo { get; set; }

            [Display(Name = "perdoruesi", ResourceType = typeof(Resource))]
            public string userId { get; set; }

        }

        public class Payments
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Salary { get; set; }
            public string Month { get; set; }
            public DateTime ExecutionDate { get; set; }
        }

    }
}
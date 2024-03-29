﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS
{
    [Authorize(Policy = "Payment:List")]
    public class ListModel : BaseModel
    {
        public ListModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) { }

        public void OnGet()
        {
            error = TempData.Get<Error>("error");
            payments = dbContext.Payments.Select(S => new Payments { 
                 Month = CultureInfo.CurrentCulture.Name=="en-US" ? S.MonthNavigation.MonthEn : S.MonthNavigation.MonthSq,
                 ExecutionDate = S.DtInserted,
                 FirstName = S.User.FirstName,
                 LastName = S.User.LastName,
                 Salary = S.Salary.ToString()
            }).ToList();
        }

        public IActionResult OnGetReport(int f)
        {
            DateTime startDate = DateTime.ParseExact("01-01-2020", "dd-MM-yyyy", null);
            DateTime endDate = DateTime.Now.AddDays(1);
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("username", "strongpassword");
                reportBytes = client.DownloadData("httpReportServerReportViewer.aspx?%2fESMSReports%2fSalariesPaid&rs:Command=Render&userId=" + null + "&dtFrom=" + startDate.ToString("yyyy-MM-dd") + "&dtTo=" + endDate.ToString("yyyy-MM-dd") + "&rs:Format=" + getFormatReport(f));
            }

            return File(reportBytes, "application/" + getFormatReport(f).ToLower(), f != 1 ? "Pagat " + DateTime.Now.ToShortDateString() + getExtension(f) : "");
        }

        public Error error { get; set; }

        public List<Payments> payments { get; set; }

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
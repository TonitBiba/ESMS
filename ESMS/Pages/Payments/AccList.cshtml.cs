using System;
using System.Collections.Generic;
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
    [Authorize(Policy = "Payment:AccList")]
    public class AccListModel : BaseModel
    {
        public AccListModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) { }

        public void OnGet()
        {
            error = TempData.Get<Error>("error");
            AccountantList = dbContext.Payments.Select(S => new AccountantListVm
            { 
                 FirstName = S.User.FirstName,
                LastName = S.User.LastName,
                IdCard = S.User.PersonalNumber,
                Taxgroup = GetTaxGroupNameFromID(S.User.TaxGroupId),
                Salaryforcalculation = S.Salary,
                EmployeePension=(decimal)S.EmployeePension,
                EmployerPension= (decimal)S.EmployerPension,
                TaxableIncome= (decimal)S.TaxableIncome,
                WithholdingTax= (decimal)S.WithholdingTax,
                NetWage= (decimal)S.NetWage,
                PositionName=S.User.JobTitle

            }).ToList();
        }

        public IActionResult OnGetReport(int f)
        {
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("reportuser", "Esms2019.");
                reportBytes = client.DownloadData("http://tonit/ReportServer/Pages/ReportViewer.aspx?%2fESMSReports%2fSalariesPaid&rs:Command=Render&rs:Format=" + getFormatReport(f));
            }

            return File(reportBytes, "application/" + getFormatReport(f).ToLower(), f != 1 ? "Pagat " + DateTime.Now.ToShortDateString() + getExtension(f) : "");
        }

        public Error error { get; set; }

        public List<AccountantListVm> AccountantList { get; set; }

        public class AccountantListVm
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

        }

        public static  string GetTaxGroupNameFromID( int taxgroupId)
        {

            switch (taxgroupId)
            {
                case 1:
                    
                    return "I rregullt me pension";

                case 2:
                    return "I rregullt pa pension";
                case 3:
                    return "Jo i rregullt me pension";

                default:
                    return "Jo i rregullt pa pension";

            }

        }

    }
}
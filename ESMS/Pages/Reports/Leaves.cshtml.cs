using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
    public class LeavesModel : BaseModel
    {
        public LeavesModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }
        
        public void OnGet()
        {

        }

        public async Task<PartialViewResult> OnPostSearch(string dtFrom, string dtTo)
        {
            DateTime startDate = DateTime.ParseExact(dtFrom, "dd-MM-yyyy", null);
            DateTime endDate = DateTime.ParseExact(dtTo, "dd-MM-yyyy", null);

            var viewModel = (from L in dbContext.Leaves
                             join LD in dbContext.LeavesDetails on L.Id equals LD.NLeaves
                             where LD.BActive == true &&
                                   L.DtInserted >= startDate && L.DtInserted<= endDate
                             select new ListViewModel
                             {
                                 EndDate = L.EndDate,
                                 FirstName = L.VcUserNavigation.FirstName,
                                 LastName = L.VcUserNavigation.LastName,
                                 LeaveReason = CultureInfo.CurrentCulture.Name=="en-US" ? L.NTypeOfLeavesNavigation.NameEn : L.NTypeOfLeavesNavigation.NameSq,
                                 StarDate = L.StartDate,
                                 LID = L.Id,
                                 Status = CultureInfo.CurrentCulture.Name == "en-US" ? LD.NStatusNavigation.NameEn : LD.NStatusNavigation.NameSq,
                                 dtInserted = L.DtInserted
                             }).ToList();

            TempData["model"] = viewModel;
            return Partial("LeavesList");
        }

        public IActionResult OnGetReport(int f, string dtFrom, string dtTo)
        {
            DateTime startDate = DateTime.ParseExact(dtFrom, "dd-MM-yyyy", null);
            DateTime endDate = DateTime.ParseExact(dtTo, "dd-MM-yyyy", null);

            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("HelloUser", "StrongPassword");
                
                reportBytes = client.DownloadData("http://tonit/ReportServer/Pages/ReportViewer.aspx?%2fESMSReports%2fLeaves&rs:Command=Render&dtFrom=" + startDate.ToString("yyyy-MM-dd") + "&dtTo=" + endDate.ToString("yyyy-MM-dd") + "&rs:Format=" + getFormatReport(f));
            }

            return File(reportBytes, "application/" + getFormatReport(f).ToLower(), f != 1 ? "Kerkesat per pushim " + DateTime.Now.ToShortDateString() + getExtension(f) : "");
        }

        public InputModel Input { get; set; }
        public class InputModel
        {
            [Display(Name = "data", ResourceType = typeof(Resource))]
            public string dtFrom { get; set; }
            public string dtTo { get; set; }
        }

        public class ListViewModel
        {
            public int LID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LeaveReason { get; set; }
            public DateTime StarDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Status { get; set; }
            public DateTime dtInserted { get; set; }
            public bool Review { get; set; }
            public bool FillIn { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.AnnualLeave
{
    [Authorize(Policy = "AnnualLeave:List")]
    public class ListModel : BaseModel
    {
        public ListModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }

        public void OnGet()
        {
            error = TempData.Get<Error>("error");
            listViewModels = (from L in dbContext.Leaves
                              join LD in dbContext.LeavesDetails on L.Id equals LD.NLeaves
                              where LD.BActive == true && L.VcUser == (!User.IsInRole("Burimet_Njerzore")?User.FindFirstValue(ClaimTypes.NameIdentifier): L.VcUser)
                              orderby LD.DtInserted descending
                              select new ListViewModel {
                                  EndDate = L.EndDate,
                                  FirstName = L.VcUserNavigation.FirstName,
                                  LastName = L.VcUserNavigation.LastName,
                                  LeaveReason = CultureInfo.CurrentCulture.Name=="en-US"? L.NTypeOfLeavesNavigation.NameSq : L.NTypeOfLeavesNavigation.NameSq,
                                  StarDate = L.StartDate,
                                  LID = L.Id,
                                  Status = CultureInfo.CurrentCulture.Name == "en-US" ? LD.NStatusNavigation.NameEn : LD.NStatusNavigation.NameSq,
                                  dtInserted = L.DtInserted,
                                  FillIn = (LD.NStatus == 4 && L.VcUser == User.FindFirstValue(ClaimTypes.NameIdentifier) ? true : false),
                                  Review = ((LD.NStatus == 1 || LD.NStatus == 5) && User.IsInRole("Burimet_Njerzore") ? true : false)
                              }).ToList();
        }

        public Error error { get; set; }

        public List<ListViewModel> listViewModels { get; set; }

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
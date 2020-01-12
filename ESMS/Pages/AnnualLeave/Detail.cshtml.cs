using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.AnnualLeave
{
    [Authorize(Policy = "AnnualLeave:Details")]
    public class DetailModel : BaseModel
    {
        public DetailModel(SignInManager<ApplicationUser> signManager, UserManager<ApplicationUser> userManager) : base(signManager, userManager) { }

        public void OnGet(string LIDEnc)
        {
            int LId = Confidenciality.Decrypt<int>(LIDEnc);
            viewModel = (from L in dbContext.Leaves
                         join LD in dbContext.LeavesDetails on L.Id equals LD.NLeaves
                         where LD.BActive == true && L.Id == LId
                         select new ViewModel
                         {
                             EndDate = L.EndDate,
                             FirstName = L.VcUserNavigation.FirstName,
                             LastName = L.VcUserNavigation.LastName,
                             LeaveReason = CultureInfo.CurrentCulture.Name == "en-US" ? L.NTypeOfLeavesNavigation.NameEn : L.NTypeOfLeavesNavigation.NameSq,
                             StarDate = L.StartDate,
                             LID = L.Id,
                             Status = CultureInfo.CurrentCulture.Name=="en-US"? LD.NStatusNavigation.NameEn : LD.NStatusNavigation.NameSq,
                             dtInserted = L.DtInserted,
                             Comment = L.VcComment
                         }).FirstOrDefault();
        }

        public IActionResult OnGetDocument(string LIDEnc)
        {
            var LID = Confidenciality.Decrypt<int>(LIDEnc);
            var filePath = dbContext.Leaves.Where(u => u.Id == LID).Select(S => new { S.VcDocumentPath }).FirstOrDefault();
            if (filePath.VcDocumentPath == null)
            {
                return RedirectToPage("Detail", new { LIDEnc = LIDEnc });
            }

            var fileBytes = ShowFile(filePath.VcDocumentPath);
            return File(fileBytes, "application/pdf", Path.GetFileName(filePath.VcDocumentPath.Replace(".zip.enc", "")));
        }

        public ViewModel viewModel { get; set; }

        public class ViewModel
        {
            public int LID { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "emri", ResourceType = typeof(Resource))]
            public string FirstName { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "mbiemri", ResourceType = typeof(Resource))]
            public string LastName { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "leaveReason", ResourceType = typeof(Resource))]
            public string LeaveReason { get; set; }

            [Display(Name = "startDateLeave", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public DateTime StarDate { get; set; }

            [Display(Name = "endDateLeave", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public DateTime EndDate { get; set; }

            [Display(Name = "statusi", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string Status { get; set; }

            [Display(Name = "dtInsertimit", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public DateTime dtInserted { get; set; }

            [Display(Name = "comment", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string Comment { get; set; }

            public IFormFile Document { get; set; }
        }
    }
}
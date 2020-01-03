using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.AnnualLeave
{
    [Authorize(Policy = "AnnualLeave:Edit")]
    public class EditModel : BaseModel
    {
        public EditModel(SignInManager<ApplicationUser> signManager, UserManager<ApplicationUser> userManager) : base(signManager, userManager) { }

        public void OnGet(string LidEnc)
        {
            int LID = Confidenciality.Decrypt<int>(LidEnc);
            Input = (from L in dbContext.Leaves
                     join LD in dbContext.LeavesDetails on L.Id equals LD.NLeaves
                     where LD.BActive == true && L.Id == LID
                     select new InputModel { 
                         TypeOfLeaves = L.NTypeOfLeaves,
                         Comment = L.VcComment,
                         EndDate = L.EndDate.ToString("yyyy-MM-dd"),
                         StartDate = L.StartDate.ToString("yyyy-MM-dd")
                     }).FirstOrDefault();    
        }

        public IActionResult OnGetDocument(string LIDEnc)
        {
            var LID = Confidenciality.Decrypt<int>(LIDEnc);
            var filePath = dbContext.Leaves.Where(u => u.Id == LID).Select(S => new { S.VcDocumentPath }).FirstOrDefault();
            if (filePath.VcDocumentPath == null)
            {
                return RedirectToPage("Read", new { LIDEnc = LIDEnc });
            }

            var fileBytes = ShowFile(filePath.VcDocumentPath);
            return File(fileBytes, "application/pdf", Path.GetFileName(filePath.VcDocumentPath.Replace(".zip", "")));
        }

        public Error error { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "leaveReason", ResourceType = typeof(Resource))]
            public int TypeOfLeaves { get; set; }

            [Display(Name = "comment", ResourceType = typeof(Resource))]
            public string Comment { get; set; }

            [Display(Name = "startDateLeave", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string StartDate { get; set; }

            [Display(Name = "endDateLeave", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string EndDate { get; set; }

            [Display(Name = "document", ResourceType = typeof(Resource))]
            public IFormFile Document { get; set; }

        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace ESMS.Pages.AnnualLeave
{
    [Authorize(Policy = "AnnualLeave:Read")]
    public class ReadModel : BaseModel
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public ReadModel(SignInManager<ApplicationUser> signManager, UserManager<ApplicationUser> userManager, IHubContext<NotificationHub> _hubContext) : base(signManager, userManager) {
            this._hubContext = _hubContext;
        }

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
                             LeaveReason = L.NTypeOfLeavesNavigation.NameSq,
                             StarDate = L.StartDate,
                             LID = L.Id,
                             Status = LD.NStatusNavigation.NameSq,
                             dtInserted = L.DtInserted
                         }).FirstOrDefault();
            Input = new InputModel { LidEnc = LIDEnc };
        }

        public IActionResult OnGetDocument(string LIDEnc)
        {
            var LID = Confidenciality.Decrypt<int>(LIDEnc);
            var filePath = dbContext.Leaves.Where(u => u.Id == LID).Select(S => new { S.VcDocumentPath}).FirstOrDefault();
            if (filePath.VcDocumentPath == null)
            {
                return RedirectToPage("Read",new { LIDEnc = LIDEnc });
            }

            var fileBytes = ShowFile(filePath.VcDocumentPath);
            return File(fileBytes, "application/pdf", Path.GetFileName(filePath.VcDocumentPath.Replace(".zip","")));
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                int LID = Confidenciality.Decrypt<int>(Input.LidEnc);
                dbContext.LeavesDetails.Where(L => L.NLeaves == LID && L.BActive).FirstOrDefault().BActive = false;

                dbContext.LeavesDetails.Add(new LeavesDetails
                {
                    BActive = true,
                    DtInserted = DateTime.Now,
                    NLeaves = LID,
                    NStatus = Input.Status,
                    VcUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    VcComment = Input.Comment
                });

                var status = dbContext.Status.Where(s => s.Id == Input.Status).Select(s => s.NameSq).FirstOrDefault();
                var UserId = dbContext.Leaves.Where(l => l.Id == LID).Select(s => s.VcUserNavigation.Id).FirstOrDefault();

               var notification = new Notifications { 
                    DtInserted = DateTime.Now,
                    Title = "Kërkesa për pushim",
                    VcIcon = "zmdi zmdi-store",
                    VcInsertedUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    VcText = "Kërkesa juaj për pushim është shqyrtuar dhe ka marrë statusin "+ status,
                    VcUser = UserId,
                };
                dbContext.Notifications.Add(notification);
                await dbContext.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync(notification.VcUser, notification.VcText, notification.Title, "info", "/");

                TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
                return RedirectToPage("List");
            }catch(Exception ex)
            {
                return RedirectToPage("Read", new { LIDEnc = Input.LidEnc });
            }
        }

        public ViewModel viewModel { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string LidEnc { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "statusi", ResourceType = typeof(Resource))]
            public int Status { get; set; }
            
            [Display(Name = "comment", ResourceType = typeof(Resource))]
            public string Comment { get; set; }

        }

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
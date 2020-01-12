using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
using Microsoft.Extensions.Configuration;

namespace ESMS.Pages.AnnualLeave
{
    [Authorize(Policy = "AnnualLeave:Edit")]
    public class EditModel : BaseModel
    {
        public IConfiguration configuration;
        
        private readonly IHubContext<NotificationHub> _hubContext;

        public EditModel(SignInManager<ApplicationUser> signManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHubContext<NotificationHub> _hubContext) : base(signManager, userManager) {
            this.configuration = configuration;
            this._hubContext = _hubContext;
        }

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
                         StartDate = L.StartDate.ToString("yyyy-MM-dd"),
                         LidEnc = LidEnc,
                         HRComment = LD.VcComment
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pathOfSavedFile = SaveFiles(Input.Document, FType.AnnualLeaveFile, configuration);

                    DateTime startDate = DateTime.ParseExact(Input.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(Input.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    int LID = Confidenciality.Decrypt<int>(Input.LidEnc);
                    dbContext.LeavesDetails.Where(L => L.NLeaves == LID && L.BActive == true).FirstOrDefault().BActive = false;

                    var leaveForChange = dbContext.Leaves.Where(L => L.Id == LID).FirstOrDefault();
                    leaveForChange.NTypeOfLeaves = Input.TypeOfLeaves;
                    leaveForChange.StartDate = startDate;
                    leaveForChange.EndDate = endDate;
                    leaveForChange.VcComment = Input.Comment;
                    leaveForChange.VcDocumentPath = pathOfSavedFile;

                    dbContext.LeavesDetails.Add(new LeavesDetails
                    {
                        NLeaves = LID,
                        BActive = true,
                        DtInserted= DateTime.Now,
                        NStatus = (int)Statuses.Completed,
                        VcUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    });

                    var notifications = dbContext.AspNetUsers.Where(u => u.AspNetUserRoles.FirstOrDefault().Role.Name == "Burimet_Njerzore").Select(U => new Notifications
                    {
                        Title = "Korigjimi i kërkesës për pushim",
                        DtInserted = DateTime.Now,
                        VcIcon = "zmdi zmdi-store",
                        VcInsertedUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        VcUser = U.Id,
                        VcText = "Është bërë korigjimi i kërkesës për pushim nga përdoruesi " + User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname)
                    }).ToList();
                    dbContext.Notifications.AddRange(notifications);
                    await dbContext.SaveChangesAsync();

                    foreach (var notification in notifications)
                        await _hubContext.Clients.All.SendAsync(notification.VcUser, notification.VcText, notification.Title, "info", "/");

                    TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
                    return RedirectToPage("List");
                }
                else
                    error = new Error { nError = 4, ErrorDescription = Resource.invalidData };
            }
            catch (Exception ex)
            {
                SaveLog(ex, HttpContext);
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return Page();
        }

        public Error error { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string HRComment { get; set; }

            public string LidEnc { get; set; }

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
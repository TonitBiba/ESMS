using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ESMS.Pages.AnnualLeave
{
    [Authorize(Policy = "AnnualLeave:Create")]
    public class CreateModel : BaseModel
    {

        public IConfiguration configuration;
        public CreateModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration) : base(signInManager, userManager) {
            this.configuration = configuration;
        }

        public void OnGet()
        {

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
                    dbContext.Leaves.Add(new Leaves
                    {
                        DtInserted = DateTime.Now,
                        NTypeOfLeaves = Input.TypeOfLeaves,
                        VcUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        VcComment = Input.Comment,
                        StartDate = startDate,
                        EndDate = endDate,
                        VcDocumentPath = pathOfSavedFile,
                        LeavesDetails = new List<LeavesDetails> { new LeavesDetails {
                             DtInserted = DateTime.Now,
                             NStatus = (int)Statuses.Applied,
                             VcUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                             BActive = true
                        }}
                    });
                    await dbContext.SaveChangesAsync();
                    
                    TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
                    return RedirectToPage("List");
                }
                else
                {
                    error = new Error { nError = 4, ErrorDescription = Resource.invalidData };
                }
            }
            catch (Exception ex)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }



            return Page();
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
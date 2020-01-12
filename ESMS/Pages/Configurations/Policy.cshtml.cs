using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.Pages.Configurations
{
    [Authorize(Policy = "Policy:Read")]
    public class PolicyModel : BaseModel
    {
        public PolicyModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) :base(signInManager, userManager) { }

        public void OnGet()
        {
            policies = dbContext.Policy.ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            policies = dbContext.Policy.ToList();
            if (!ModelState.IsValid)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgRuajtjaSukses };
                return Page();
            }
            try
            {
                if(dbContext.Policy.Any(P=>P.VcPolicyName == Input.policyName))
                {
                    error = new Error { nError = 4, ErrorDescription = Resource.msgDuplicateData };
                    return Page();
                }
                dbContext.Policy.Add(new Policy
                {
                    VcPolicyName = Input.policyName,
                    VcClaimValue = Input.ClaimValue,
                    VcClaimType = Input.ClaimType,
                    BActive = true,
                    DtInserted = DateTime.Now,
                    NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                });
                await dbContext.SaveChangesAsync();

                error = new Error { nError = 1, ErrorDescription= Resource.msgRuajtjaSukses };
                policies = dbContext.Policy.ToList();
            }
            catch (Exception ex)
            {
                SaveLog(ex, HttpContext);
                dbContext = new ESMSContext();
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return Page();
        }

        public Error error { get; set; }

        public IList<Policy> policies { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "policy", ResourceType = typeof(Resource))]
            public string policyName { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "claim", ResourceType = typeof(Resource))]
            public string ClaimType { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "claimValue", ResourceType = typeof(Resource))]
            public string ClaimValue { get; set; }
        }
    }
}
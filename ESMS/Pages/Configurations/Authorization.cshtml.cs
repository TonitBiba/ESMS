using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ESMS.Pages.Configurations
{
    [Authorize(Policy = "ReadAutorizimet")]
    public class AuthorizationModel : BaseModel
    {
        public AuthorizationModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager)
        { }

        public void OnGet()
        {

        }

    }
}
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;

namespace ESMS.Pages.Configurations
{
    [Authorize(Policy = "ReadAutorizimet")]
    public class AuthorizationModel : BaseModel
    {
        public void OnGet()
        {

        }

    }
}
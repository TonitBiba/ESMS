using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS
{
    public class ReadModel : BaseModel
    {

        public ReadModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) { }

        public void OnGet()
        {

        }
    }
}
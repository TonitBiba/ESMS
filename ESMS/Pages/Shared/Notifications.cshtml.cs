using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS
{
    public class NotificationsModel : BaseModel
    {
        public NotificationsModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }

        public void OnGet()
        {
            notifications = dbContext.Notifications.Where(N=>N.VcUser == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(d=>d.DtInserted).ToList();
        }
        public List<Notifications> notifications { get; set; }
    }
}
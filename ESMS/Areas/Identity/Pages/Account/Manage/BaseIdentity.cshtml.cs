using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Areas.Identity.Pages.Account.Manage
{
    public class BaseIdentityModel : PageModel
    {
        protected SignInManager<ApplicationUser> signInManager;
        protected UserManager<ApplicationUser> userManager;

        public static byte[] userProfile;

        protected ESMSContext dbContext = null;

        public BaseIdentityModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            dbContext = new ESMSContext();
        }


        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var user = dbContext.AspNetUsers.Where(U => U.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(U => new { U.UserProfile, U.ChangePassword }).FirstOrDefault();
            userProfile = user.UserProfile;
            signInManager.RefreshSignInAsync(userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result).ConfigureAwait(false);
            if (user.ChangePassword == true)
            {
                context.HttpContext.Response.Redirect("/Identity/Account/Manage/ChangePassword");
            }
            dbContext.Logs.Add(new Logs
            {
                DtInserted = DateTime.Now,
                Hostname = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                IpAdress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                Method = context.HttpContext.Request.Method,
                Page = context.ActionDescriptor.RouteValues.Values.FirstOrDefault().ToString(),
                StatusCode = context.HttpContext.Response.StatusCode,
                Url = context.ActionDescriptor.RelativePath,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });
            dbContext.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using ESMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ESMS.Pages
{
    [Authorize]
    public class IndexModel : BaseModel
    {
        private readonly IEmailSender _emailSender;

        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender _emailSender) :base(signInManager, userManager)
        {
            this._emailSender = _emailSender;
        }

        public async Task OnGet()
        {
            listLogs = dbContext.Logs.Where(L=>L.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(L => new Logs 
            { 
                dtInserted = (DateTime)L.DtInserted,  
                HostName = L.Hostname,
                IpAdress = L.IpAdress,
                status = (int)L.StatusCode,
                Url = L.Url
            }).OrderByDescending(L=>L.dtInserted).Take(100).ToList();

            if (User.IsInRole("Administrator"))
            {
                statistics = new List<StatisticsModel> {
                     new StatisticsModel{ Amount = (dbContext.AspNetUsers.Count() - 1).ToString(), Icon = "zmdi zmdi-account-o", Title = Resource.numriPerdoruesve},
                };
            }else if (User.IsInRole("Programmer"))
            {
                statistics = new List<StatisticsModel> {
                     new StatisticsModel{ Amount = String.Format("{0:C}", dbContext.AspNetUsers.Where(U=>U.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(U=>U.Salary).FirstOrDefault()).Substring(1)+" €", Icon = "zmdi zmdi-money", Title = Resource.paga}                };
            }else if (User.IsInRole("Burimet_Njerzore"))
            {
                statistics = new List<StatisticsModel> {
                     new StatisticsModel{ Amount = (dbContext.AspNetUsers.Count() - 1).ToString(), Icon = "zmdi zmdi-account-o", Title = Resource.numriPerdoruesve},
                     new StatisticsModel{Amount = String.Format("{0:C}", dbContext.AspNetUsers.Where(S=>S.EmployeeStatus == 1).Sum(S=>S.Salary)).Substring(1)+" €", Icon = "zmdi zmdi-money", Title = Resource.shpenzimetPaga}
                };
            }
        }

        public IActionResult OnGetLanguage(string culture, string returnUrl)
        {
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;


            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }

        public List<Logs> listLogs { get; set; }
        public class Logs
        {
            public string IpAdress { get; set; }
            public string HostName { get; set; }
            public string Url { get; set; }
            public DateTime dtInserted { get; set; }
            public int status { get; set; }
        }

        public List<StatisticsModel> statistics { get; set; }


        public class StatisticsModel
        {
            public string Icon { get; set; }
            public string Title { get; set; }
            public string Amount { get; set; }
        }

    }
}

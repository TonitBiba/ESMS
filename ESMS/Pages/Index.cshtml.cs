using System;
using System.Collections.Generic;
using System.Globalization;
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
                     new StatisticsModel{ color ="statistic__item--green", Amount = (dbContext.AspNetUsers.Count() - 1).ToString(), Icon = "zmdi zmdi-account-o", Title = Resource.numriPerdoruesve},
                };
            }else if (User.IsInRole("Programmer"))
            {
                statistics = new List<StatisticsModel> {
                     new StatisticsModel{ color ="statistic__item--green", Amount = string.Format(new CultureInfo("en-US"), "{0:C}", dbContext.AspNetUsers.Where(U=>U.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(U=>U.Salary).FirstOrDefault()).Substring(1)+" €", Icon = "zmdi zmdi-money", Title = Resource.paga}                };
            }else if (User.IsInRole("Burimet_Njerzore"))
            {
                TempData["IT"] = dbContext.AspNetUsers.Where(U => new string[] { "be007199-39b1-4557-b10f-cc4e6dc47b49", "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e" }.Contains(U.AspNetUserRoles.FirstOrDefault().RoleId)).Sum(S => S.Salary);
                TempData["Financa"] = dbContext.AspNetUsers.Where(U => new string[] { "dbc05ab9-f41f-493f-b3e6-689d14e88dda", "423a5ce2-3024-47d1-b486-4dcd3951871b" }.Contains(U.AspNetUserRoles.FirstOrDefault().RoleId)).Sum(S => S.Salary);
                DateTime dateTime = DateTime.Now;
                statistics = new List<StatisticsModel> {
                     new StatisticsModel{ color ="statistic__item--green", Amount = (dbContext.AspNetUsers.Count() - 1).ToString(), Icon = "zmdi zmdi-account-o", Title = Resource.numriPerdoruesve},
                     new StatisticsModel{ color="statistic__item--orange", Amount = string.Format(new CultureInfo("en-US"), "{0:C}", dbContext.AspNetUsers.Where(S=>S.EmployeeStatus == 1).Sum(S=>S.Salary)).Substring(1)+" €", Icon = "zmdi zmdi-money", Title = Resource.shpenzimetPaga},
                     new StatisticsModel {color="statistic__item--blue", Amount = dbContext.LeavesDetails.Where(L=>L.BActive == true && (L.NStatus == 1 || L.NStatus == 5)).Count()+"", Title = "Kërkesë për pushim", Icon = "zmdi zmdi-assignment-o" },
                     new StatisticsModel { color ="statistic__item--red", Amount = (from L in dbContext.Leaves
                                                    join LD in dbContext.LeavesDetails on L.Id equals LD.NLeaves
                                                    where LD.BActive == true && LD.NStatus == 2
                                                        && L.EndDate > dateTime
                                                        && L.StartDate < dateTime
                                                    select L.Id).Count()+"", Title = "Punëtorë në pushim", Icon = "zmdi zmdi-assignment-o" }
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
            int languId = culture == "en-US" ? 2 : 1;
            dbContext.AspNetUsers.Where(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().Language = languId;
            dbContext.SaveChanges();
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
            public string color { get; set; }
            public string Icon { get; set; }
            public string Title { get; set; }
            public string Amount { get; set; }
        }

    }
}

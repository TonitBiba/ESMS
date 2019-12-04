﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ESMS.Pages
{
    [Authorize]
    public class IndexModel : BaseModel
    {

        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager)
        {}

        public void OnGet()
        {
            listLogs = dbContext.Logs.Where(L=>L.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(L => new Logs 
            { 
                dtInserted = (DateTime)L.DtInserted,  
                HostName = L.Hostname,
                IpAdress = L.IpAdress,
                status = (int)L.StatusCode,
                Url = L.Url
            }).OrderByDescending(L=>L.dtInserted).Take(100).ToList();

            //if (User.IsInRole("Programmer"))
            //{
            //    statistics = new List<StatisticsModel> {
            //         new StatisticsModel{ Amount = "4332", Icon = }
                
            //    };
            //}


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

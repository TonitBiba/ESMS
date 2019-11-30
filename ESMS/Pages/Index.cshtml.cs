﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ESMS.Pages
{
    [Authorize]
    public class IndexModel : BaseModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            listLogs = dbContext.Logs.Select(L => new Logs 
            { 
                dtInserted = (DateTime)L.DtInserted,  
                HostName = L.Hostname,
                IpAdress = L.IpAdress,
                status = (int)L.StatusCode,
                Url = L.Url
            }).OrderByDescending(L=>L.dtInserted).Take(100).ToList();
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
    }
}

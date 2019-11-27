using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace ESMS.Pages.Shared
{
    [Authorize]
    public class BaseModel : PageModel
    {
        protected ESMSContext dbContext=null;

        public IConfiguration configuration { get; set; }

        public BaseModel()
        {
            dbContext = new ESMSContext();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
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


        public static List<SelectListItem> GetGroups()
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                return dbContext.AspNetRoles.Select(R => new SelectListItem { Text = R.Name, Value = R.Id }).ToList();
            }
        }

        public static List<SelectListItem> GetGenders()
        {
            return new List<SelectListItem> { new SelectListItem { Text = Resource.mashkull, Value = "1" }, new SelectListItem { Text = Resource.femer, Value = "2" }};
        }

        public static List<SelectListItem> GetContries()
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                return dbContext.Contries.Select(R => new SelectListItem { Text = R.Name, Value = R.Id.ToString()}).ToList();
            }
        }

        protected string SaveFiles(IFormFile file, FType fileType)
        {
            string path = "";
            if(fileType == FType.ContractFile)
                path = configuration.GetSection("AppSettings").GetSection("ContractFiles").Value;
            else
                path = configuration.GetSection("AppSettings").GetSection("GeneralFiles").Value;
            path += Guid.NewGuid().ToString() + " - " + file.FileName+".zip";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            using (var stream = file.OpenReadStream())
            using (var zsp = new GZipStream(fs, CompressionMode.Compress))
                stream.CopyTo(zsp);
            fs.Close();
            return path;
        }

        protected enum FType
        {
            ContractFile = 1,
            GeneralFile = 2
        }

    }
}
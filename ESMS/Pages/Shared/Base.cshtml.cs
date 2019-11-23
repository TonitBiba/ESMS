using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Extensions.DependencyInjection;

namespace ESMS.Pages.Shared
{
    [Authorize]
    public class BaseModel : PageModel
    {
        protected ESMSContext dbContext=null;

        protected IConfiguration configuration { get; set; }

        public BaseModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

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

        protected string SaveFiles(IFormFile file, FType fileType)
        {
            string fullPath = "";
            if(fileType == FType.ContractFile)
            {
                fullPath = @"Contract\" + Guid.NewGuid().ToString() + file.FileName;
            }
            else
            {
                fullPath = configuration.GetSection("AppSettings").GetSection("GeneralFiles").Value + "/"+ Guid.NewGuid().ToString() + file.ContentType;
            }
            byte[] fileByte = new byte[file.Length];
            file.OpenReadStream().Read(fileByte);
            FileStream fs = new FileStream(fullPath, FileMode.Create);
            fs.Write(fileByte);
            fs.Close();
            return fullPath;
        }

        protected enum FType
        {
            ContractFile = 1,
            GeneralFile = 2
        }

    }
}
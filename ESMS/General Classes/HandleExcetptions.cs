using ESMS.Data.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.General_Classes
{
    public class HandleExcetptions : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            using (ESMSContext esms = new ESMSContext())
            {
                esms.Logs.Add(new Logs
                {
                     BError = true,
                     DtInserted = DateTime.Now,
                     Exception = context.Exception.Message,
                     Hostname = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                     IpAdress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                     Method = context.HttpContext.Request.Method,
                     Page = context.ActionDescriptor.RouteValues.Values.FirstOrDefault().ToString(),
                     StatusCode = context.HttpContext.Response.StatusCode,
                     Url = context.HttpContext.Request.Path.Value,
                     UserId = null,
                     ExceptionDetail = context.Exception.StackTrace
                });
                esms.SaveChanges();
                context.ExceptionHandled = true;
                context.HttpContext.Response.Redirect("Error");
            }
        }
    }
}

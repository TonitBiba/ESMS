using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace ESMS.Pages.Shared
{
    [Authorize]
    public class BaseModel : PageModel
    {
        protected ESMSContext dbContext=null;
        protected IServiceCollection services;

        public BaseModel()
        {
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            dbContext = new ESMSContext();
            
        }


        public static List<SelectListItem> GetGroups()
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                return dbContext.AspNetRoles.Select(R => new SelectListItem { Text = R.Name, Value = R.Id }).ToList();
            }
        }

    }
}
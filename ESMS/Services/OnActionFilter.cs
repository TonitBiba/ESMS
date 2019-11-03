using ESMS.Data.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESMS.Services
{
    public class OnActionFilter : IPageFilter
    {
        ESMSContext dbContext = null;
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // do something during page handler selection.
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            dbContext = new ESMSContext();
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            // do something after a handler method executes.
        }
    }
}


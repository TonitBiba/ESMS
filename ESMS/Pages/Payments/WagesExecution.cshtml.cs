using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Payments
{
    [Authorize(Policy = "ExecuteWages")]
    public class WagesExecutionModel : BaseModel
    {
        public void OnGet()
        {

        }
    }
}
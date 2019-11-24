using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ESMS.Pages.Employees
{
    public class ListModel : BaseModel
    {

        public void OnGet()
        {

        }

        public async Task<IActionResult> onPostAsync()
        {


            return Page();
        }
    }
}
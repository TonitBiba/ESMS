using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESMS
{
    public class ReadModel : BaseModel
    {
        public ReadModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }

        public void OnGet(int rId)
        {
            Input = new InputModel { ReportType = rId };

        }

        public IActionResult OnGetReportParameters(int ReportType)
        {
            switch (ReportType)
            {
                case 1:
                    return RedirectToPage("Employe");
                case 2:
                    return RedirectToPage("Payments");
                case 3:
                    return RedirectToPage("Leaves");
                default:
                    break;
            }
            return Page();
        }

        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "reportType", ResourceType =typeof(Resource))]
            public int ReportType { get; set; }
        }


    }
}
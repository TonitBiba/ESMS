using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS
{
    [Authorize(Policy = "Payment:List")]
    public class ListModel : BaseModel
    {
        public ListModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) { }

        public void OnGet()
        {
            error = TempData.Get<Error>("error");
            payments = dbContext.Payments.Select(S => new Payments { 
                 Month = S.MonthNavigation.MonthSq,
                 ExecutionDate = S.DtInserted
            }).Distinct().ToList();
        }

        public Error error { get; set; }

        public List<Payments> payments { get; set; }

        public class Payments
        {
            public string Month { get; set; }
            public DateTime ExecutionDate { get; set; }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS
{
    [Authorize(Policy = "Payment:Create")]
    public class CreateModel : BaseModel
    {
        public CreateModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager):base(signInManager, userManager) { }

        public void OnGet()
        {
            Input = dbContext.AspNetUsers.Select(U => new Employee
            {
                FirstName = U.FirstName,
                LastName = U.LastName,
                salary = (decimal)U.Salary
            }).ToList();
        }

        public async Task<IActionResult> OnPost()
        {

            return Page();
        }


        [BindProperty]
        public List<Employee> Input { get; set; }

        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public decimal salary { get; set; }
            public int MyProperty { get; set; }





        }


    }
}
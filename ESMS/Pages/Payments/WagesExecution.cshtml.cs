using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ESMS.Pages.Payments
{
    [Authorize(Policy = "ExecuteWages")]
    public class WagesExecutionModel : BaseModel
    {
        public IConfiguration configuration;

        public WagesExecutionModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration) : base(signInManager, userManager) {
            this.configuration = configuration;
            
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            var path = SaveFiles(Input.file, FType.ContractFile, configuration);
            var fileBytes = ShowFile(path);
            return File(fileBytes, "application/pdf");
        }


        [BindProperty]
        public InputClass Input { get; set; }

        public class InputClass
        {
            public IFormFile file { get; set; }
        }

    }
}
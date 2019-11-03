using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Data.Model;
using ESMS.Pages.Shared;
using ESMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Configurations
{
    [Authorize]
    public class MenuModel : BaseModel
    {

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            dbContext.Menu.Add(new Menu { DtInserted = DateTime.Now, VcMenNameSq = Input.MenuName, NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            await dbContext.SaveChangesAsync();
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "emriMenus", ResourceType = typeof(Resource))]
            public string MenuName { get; set; }

            [Required]
            [Display(Name ="controller", ResourceType = typeof(Resource))]
            public string Controller { get; set; }

            [Display(Name = "metoda", ResourceType =typeof(Resource))]
            public string Method { get; set; }

            [Display(Name = "ikona", ResourceType = typeof(Resource))]
            public string Icon { get; set; }

        }


    }
}
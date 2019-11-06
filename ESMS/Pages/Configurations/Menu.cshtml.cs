using ESMS.Data.Model;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.Pages.Configurations
{
    [Authorize]
    public class MenuModel : BaseModel
    {

        public void OnGet()
        {
            string cipertext = Confidenciality.Enkrypt(10);
            int plaintext = Confidenciality.Decrypt<int>(cipertext);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            dbContext.Menu.Add(new Menu { VcMenNameSq = Input.MenuName_Sq, VcMenuNameEn = Input.MenuName_En, DtInserted = DateTime.Now, NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            await dbContext.SaveChangesAsync();
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "emriMenusShqip", ResourceType = typeof(Resource))]
            public string MenuName_Sq { get; set; }

            [Required]
            [Display(Name = "emriMenusAnglisht", ResourceType = typeof(Resource))]
            public string MenuName_En { get; set; }

            [Display(Name = "ikona", ResourceType = typeof(Resource))]
            public string Icon { get; set; }

        }


    }
}
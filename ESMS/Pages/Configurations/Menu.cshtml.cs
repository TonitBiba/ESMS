using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.Pages.Configurations
{
    [Authorize(Policy = "ReadMenu")]
    public class MenuModel : BaseModel
    {

        public void OnGet()
        {
            menus = dbContext.Menu.ToList();
            subMenus = dbContext.SubMenu.ToList();
            error = TempData.Get<Error>("error");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                dbContext.Menu.Add(new Menu { VcMenNameSq = Input.MenuName_Sq, VcMenuNameEn = Input.MenuName_En, VcIcon = Input.Icon, DtInserted = DateTime.Now, NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
                await dbContext.SaveChangesAsync();
                error = new Error { nError = 1, errorDescription = "Te dhenat jane regjistruar me sukses!" };
            }catch(Exception ex)
            {
                dbContext = new ESMSContext();
                error = new Error { nError = 4, errorDescription = "Ka ndodhur nje gabim gjate ruajtjes se te dhenave!" };
            }
            menus = dbContext.Menu.ToList();
            return Page();
        }

        public JsonResult OnPostFshije(string MEnc)
        {
            Error error = new Error { nError = 1, errorDescription = "Te dhenat jane ruajtur me sukses!" };
            try
            {
                int menuId = Confidenciality.Decrypt<int>(MEnc);
                dbContext.Menu.Remove(dbContext.Menu.Find(menuId));
                dbContext.SaveChanges();
            }catch(Exception ex)
            {
                error = new Error { nError = 4, errorDescription = "Ka ndodhur nje gabim gjate ruajtjes!" };
            }
            return new JsonResult(error);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<Menu> menus { get; set; }

        public IList<SubMenu> subMenus { get; set; }

        public Error error { get; set; }

        public class Error
        {
            public int nError { get; set; }
            public string errorDescription { get; set; }
        }

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
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.Pages.Configurations
{
    [Authorize(Roles = "Administrator")]
    public class _MenuEditModel : BaseModel
    {
        public void OnGet(string MEnc)
        {
            int menuId = Confidenciality.Decrypt<int>(MEnc);
            Input = dbContext.Menu.Where(M => M.NMenuId == menuId).Select(M => new InputModel { Icon = M.VcIcon, MenuName_En = M.VcMenuNameEn, MenuName_Sq = M.VcMenNameSq, MEnc = MEnc }).FirstOrDefault();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                int menuId = Confidenciality.Decrypt<int>(Input.MEnc);
                var menuToChange = dbContext.Menu.Where(t => t.NMenuId == menuId).FirstOrDefault();
                menuToChange.VcMenNameSq = Input.MenuName_Sq;
                menuToChange.VcMenuNameEn = Input.MenuName_En;
                menuToChange.VcIcon = Input.Icon;
                menuToChange.DtModify = DateTime.Now;
                menuToChange.NModifyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await dbContext.SaveChangesAsync();

                TempData.Set("error", new Error { nError = 1, ErrorDescription = "Te dhenat jane ruajtur me sukses!" });
            }
            catch (Exception ex)
            {
                TempData.Set("error", new Error { nError = 4, ErrorDescription = "Ka ndodhur nje gabim gjate ruajtjes!" });
            }
            return RedirectToPage("./Menu");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string MEnc { get; set; }
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
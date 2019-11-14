using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Configurations
{
    public class _RegisterSubMenuModel : BaseModel
    {
        public void OnGet(string MEncId)
        {
            int menuId = Confidenciality.Decrypt<int>(MEncId);
            input = new InputModel
            { 
                MenuName = dbContext.Menu.Where(t=>t.NMenuId == menuId).FirstOrDefault().VcMenNameSq,
                MEnc = MEncId
            };
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                int menuId = Confidenciality.Decrypt<int>(input.MEnc);
                dbContext.SubMenu.Add(new SubMenu
                {
                    DtInserted = DateTime.Now,
                    NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    NMenuId = menuId,
                    VcController = input.Controller,
                    VcPage = input.Page,
                    VcSubMenuSq = input.SubMenu_Sq,
                    VcSubMenuEn = input.SubMenu_En
                });
                await dbContext.SaveChangesAsync();
                TempData.Set("error", new Error { nError = 1, ErrorDescription = "Te dhenat jane ruajtur me sukses!" });
            }
            catch (Exception ex)
            {
                TempData.Set("error", new Error { nError = 4, ErrorDescription = "Ka ndodhur nje gabim gjate ruajtjes!" });
            }
            return RedirectToPage("Menu");
        }

        [BindProperty]
        public InputModel input { get; set; }

        public class InputModel
        {
            public string MEnc { get; set; }

            [Display(Name = "emriMenus", ResourceType = typeof(Resource))]
            public string MenuName { get; set; }

            [Display(Name ="Emri nen menuse")]
            [Required]
            public string SubMenu_Sq { get; set; }
            [Required]
            public string SubMenu_En { get; set; }
            [Required]
            public string Controller { get; set; }
            [Required]
            public string Page { get; set; }
        }
    }
}
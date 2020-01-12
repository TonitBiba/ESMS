using ESMS.Areas.Identity;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ESMS.Pages.Configurations
{
    [Authorize(Policy = "SubMenu:Edit")]
    public class _SubMenuEditModel : BaseModel
    {
        public _SubMenuEditModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) { }

        public void OnGet(string SMEnc)
        {
            int subMenuId = Confidenciality.Decrypt<int>(SMEnc);
            Input = dbContext.SubMenu.Where(S => S.NSubMenuId == subMenuId).Select(S => new InputModel
            {
                Controller = S.VcController,
                Page = S.VcPage,
                MenuName = CultureInfo.CurrentCulture.Name=="en-US" ? S.NMenu.VcMenuNameEn : S.NMenu.VcMenNameSq,
                SubMenu_En = S.VcSubMenuEn,
                SubMenu_Sq = S.VcSubMenuSq,
                SMEnc = SMEnc
            }).FirstOrDefault();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                int subMenuId = Confidenciality.Decrypt<int>(Input.SMEnc);
                if (ModelState.IsValid)
                {
                    var subMenu = dbContext.SubMenu.Where(S => S.NSubMenuId == subMenuId).FirstOrDefault();
                    subMenu.VcController = Input.Controller;
                    subMenu.VcPage = Input.Page;
                    subMenu.VcSubMenuSq = Input.SubMenu_Sq;
                    subMenu.VcSubMenuEn = Input.SubMenu_En;
                    subMenu.DtModify = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                }
                TempData.Set("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
            }
            catch(Exception ex)
            {
                SaveLog(ex, HttpContext);
                TempData.Set("error", new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja });
            }
            return RedirectToPage("./Menu");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SMEnc { get; set; }

            [Display(Name = "emriMenus", ResourceType = typeof(Resource))]
            public string MenuName { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "emriNenMenusSQ", ResourceType = typeof(Resource))]
            public string SubMenu_Sq { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "emriNenMenusEN", ResourceType = typeof(Resource))]
            public string SubMenu_En { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "controller", ResourceType = typeof(Resource))]
            public string Controller { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "page", ResourceType = typeof(Resource))]
            public string Page { get; set; }
        }
    }
}
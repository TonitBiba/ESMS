using ESMS.Areas.Identity;
using ESMS.Data.Model;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.Pages.Configurations
{
    [Authorize(Policy = "SubMenu:Create")]
    public class _RegisterSubMenuModel : BaseModel
    {
        public _RegisterSubMenuModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(signInManager, userManager) { }

        public void OnGet(string MEncId)
        {
            int menuId = Confidenciality.Decrypt<int>(MEncId);
            Input = new InputModel
            {
                MenuName = dbContext.Menu.Where(t => t.NMenuId == menuId).Select(S=>CultureInfo.CurrentCulture.Name=="en-US"?S.VcMenuNameEn:S.VcMenNameSq).FirstOrDefault(),
                MEnc = MEncId
            };
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if(dbContext.SubMenu.Any(S=>S.VcClaim == Input.Claim))
                {
                    TempData.Set("error", new Error { nError = 4, ErrorDescription = Resource.claimRegisterError });
                    return RedirectToPage("Menu");
                }
                int menuId = Confidenciality.Decrypt<int>(Input.MEnc);
                dbContext.SubMenu.Add(new SubMenu
                {
                    DtInserted = DateTime.Now,
                    NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    NMenuId = menuId,
                    VcController = Input.Controller,
                    VcPage = Input.Page,
                    VcSubMenuSq = Input.SubMenu_Sq,
                    VcSubMenuEn = Input.SubMenu_En,
                    VcClaim = Input.Claim
                });
                await dbContext.SaveChangesAsync();
                TempData.Set("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
            }
            catch (Exception ex)
            {
                SaveLog(ex, HttpContext);
                TempData.Set("error", new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja });
            }
            return RedirectToPage("Menu");
        }

        public async Task<JsonResult> OnPostDeleteSub(string SMEnc)
        {
            Error error = new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses };

            try
            {
                int subMenuId = Confidenciality.Decrypt<int>(SMEnc);
                dbContext.SubMenu.Remove(dbContext.SubMenu.Where(S => S.NSubMenuId == subMenuId).FirstOrDefault());
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                SaveLog(ex, HttpContext);
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return new JsonResult(error);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string MEnc { get; set; }

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

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "claim", ResourceType = typeof(Resource))]
            public string Claim { get; set; }
        }
    }
}
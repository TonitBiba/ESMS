using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace ESMS.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.Password)]
            [Display(Name = "fjalekalimiAktual", ResourceType = typeof(Resource))]
            public string OldPassword { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(100, ErrorMessageResourceName = "gjatesiaFjalekalimit", MinimumLength = 6, ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.Password)]
            [Display(Name = "fjalekalimiRi", ResourceType = typeof(Resource))]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "konfirmoFjalekalimin", ResourceType = typeof(Resource))]
            [Compare("NewPassword", ErrorMessageResourceName = "fjalekalimiNukPerputhet", ErrorMessageResourceType = typeof(Resource))]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            using (ESMSContext ESMS = new ESMSContext())
            {
                TempData["changePassword"] = ESMS.AspNetUsers.Where(U => U.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().ChangePassword;
            }
            var hasPassword = await _userManager.HasPasswordAsync(user);
            return !hasPassword ? RedirectToPage("./SetPassword") : (IActionResult)Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Fjalëkalimi është ndryshuar me sukses!";
            using (ESMSContext ESMS = new ESMSContext())
            {
                var userDetail = ESMS.AspNetUsers.Where(U => U.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault().ChangePassword = false;
                ESMS.SaveChanges();
            }
                return RedirectToPage();
        }
    }
}

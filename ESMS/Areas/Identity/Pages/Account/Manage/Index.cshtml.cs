using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "username", ResourceType = typeof(Resource))]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone(ErrorMessageResourceName = "formatiNrTelGabim", ErrorMessageResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "nrTel", ResourceType = typeof(Resource))]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "emri", ResourceType = typeof(Resource))]
            [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "nrKaraktereve", ErrorMessageResourceType = typeof(Resource))]
            public string FirstName { get; set; }

            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [Display(Name = "mbiemri", ResourceType = typeof(Resource))]
            [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "nrKaraktereve", ErrorMessageResourceType = typeof(Resource))]
            public string LastName { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            using (ESMSContext eSMSContext = new ESMSContext()) {
                var dataToBeChanged = eSMSContext.AspNetUsers.Where(A => A.Id == _userManager.GetUserId(User)).FirstOrDefault();
                dataToBeChanged.FirstName = Input.FirstName;
                dataToBeChanged.LastName = Input.LastName;
                await eSMSContext.SaveChangesAsync();
            }


            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = Resource.perditesimiMeSukses;
            return RedirectToPage();
        }
    }
}

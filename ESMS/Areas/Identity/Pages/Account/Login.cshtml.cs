﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace ESMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "emailAdresa", ResourceType = typeof(Resource))]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "fjalekalimi", ResourceType = typeof(Resource))]
            public string Password { get; set; }

            [Display(Name = "meKujto", ResourceType = typeof(Resource))]
            public bool RememberMe { get; set; }

            public string gRecaptchaResponse { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                string SECRETKEY = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SecretKey"];
                string userResponse = Input.gRecaptchaResponse;

                var webClient = new WebClient();

                string verification = webClient.DownloadString("https://www.google.com/recaptcha/api/siteverify?" + "secret=" + SECRETKEY + "&response=" + userResponse);

                var verificationJson = JsonConvert.DeserializeObject<CaptchaResponse>(verification);
                if (verificationJson.success)
                {

                    string username = Input.Email;
                    if (Input.Email.Contains("@"))
                    {
                        var user = await _userManager.FindByEmailAsync(Input.Email);
                        username = user.UserName;
                    }
                    try
                    {
                        var result = await _signInManager.PasswordSignInAsync(username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, Resource.loginError);
                            return Page();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, Resource.exceptionLogin);
                        return Page();
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, Resource.captchaError);
                    return Page();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, Resource.emailVerificationSend);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Konfirmo imellën",
                $"Ju lutem konfirmoni llogarinë duke <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikuar këtu</a>.");

            ModelState.AddModelError(string.Empty, Resource.emailVerificationSend);
            return Page();
        }
        public class CaptchaResponse
        {
            public bool success { get; set; }
            public string challenge_ts { get; set; }
            public string hostname { get; set; }
            public float score { get; set; }
            public string action { get; set; }
        }
    }
}

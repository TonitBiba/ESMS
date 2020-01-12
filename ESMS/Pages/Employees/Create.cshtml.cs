using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESMS.Pages.Employees
{
    [Authorize(Policy = "Employee:Create")]
    public class CreateModel : BaseModel
    {
        private readonly IEmailSender _emailSender;

        public IConfiguration configuration;

        public CreateModel( UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<CreateModel> logger, IEmailSender emailSender, IConfiguration configuration):base(signInManager, userManager)
        {
            _emailSender = emailSender;
            this.configuration = configuration;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            error = new Error { };
            try
            {
                if (ModelState.IsValid)
                {
                    if (Input.Contract.Length / (1024 * 1024) <= 1)
                    {
                        if (!dbContext.AspNetUsers.Any(U => U.Email == Input.EmailAdress))
                        {
                            DateTime birthday = DateTime.ParseExact(Input.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateTime DtFromEmploy = DateTime.ParseExact(Input.DtFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateTime DtToEmploy = DateTime.ParseExact(Input.DtTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            byte[] imgBytes = new byte[(int)Input.UserProfileImg.Length];
                            if (Input.UserProfileImg != null)
                                using (BinaryReader imgProfile = new BinaryReader(Input.UserProfileImg.OpenReadStream()))
                                    imgBytes = imgProfile.ReadBytes((int)Input.UserProfileImg.Length);
                            var user = new ApplicationUser
                            {
                                Email = Input.EmailAdress,
                                Address = Input.Adress,
                                Address2 = Input.AdressOpsional,
                                BirthDate = birthday,
                                City = Input.City,
                                Country = Input.Contry,
                                FirstName = Input.FirstName,
                                UserName = Input.FirstName + "." + Input.LastName,
                                LastName = Input.LastName,
                                IbanCode = Input.IBANCode,
                                Gender = Input.Gender,
                                JobTitle = Input.JobTitle,
                                PersonalNumber = Input.PersonalNumber,
                                EmployeeStatus = 1,
                                EmailConfirmed = false,
                                AccessFailedCount = 0,
                                LockoutEnabled = true,
                                DtFrom = DtFromEmploy,
                                DtTo = DtToEmploy,
                                PostCode = Input.PostalCode,
                                PhoneNumber = Input.PhoneNumber,
                                salary = Input.salary,
                                UserProfile = imgBytes,
                                TaxGroupId = Input.TaxGroupId,
                                language = 1,
                                ChangePassword = true
                            };

                            var result = await userManager.CreateAsync(user, Input.PersonalNumber);

                            if (!result.Succeeded)
                                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
                            else
                            {
                                string role = dbContext.AspNetRoles.Where(R => R.Id == Input.Position).FirstOrDefault().Name;
                                await userManager.AddToRoleAsync(user, role);
                                foreach (var claim in dbContext.AspNetRoleClaims.Where(R => R.Role.Id == Input.Position).ToList())
                                    await userManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue));

                                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                var callbackUrl = Url.Page(
                                    "/Account/ConfirmEmail",
                                    pageHandler: null,
                                    values: new { area = "Identity", userId = user.Id, code = code },
                                    protocol: Request.Scheme);

                                await _emailSender.SendEmailAsync(Input.EmailAdress, "Konfirmimi llogarisë", "Përshëndetje <b>" + Input.FirstName + " " + Input.LastName + "</b>, " +
                                    "<p> Në sistemin për menaxhim të pagave - <b> ESMS </b>, është krijuar një llogarinë me email adresën tuaj.</p>" +
                                    "Në rast se ju jeni pjesë e kompanisë \"Test company\", mund të konfirmoni llogarinë tuaj duke klikuar në linkun:" +
                                    "<a href='" + callbackUrl + "'> Konfirmo llogarinë </a>" +
                                    "<p> Ky eshte nje email i automatizuar, ju lusim të mos ktheni përgjigje në këtë email.</p>" +
                                    "Me respekt," +
                                    "<p>ESMS</p>");


                                var pathOfSavedFile = SaveFiles(Input.Contract, FType.ContractFile, configuration);
                                dbContext.EmployeeDocuments.Add(new EmployeeDocuments
                                {
                                    DtInserted = DateTime.Now,
                                    NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                                    Employee = user.Id,
                                    Name = Input.Contract.FileName,
                                    Path = pathOfSavedFile,
                                    Type = (int)FType.ContractFile
                                });
                                await dbContext.SaveChangesAsync();

                                TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses });
                                return RedirectToPage("List");
                            }
                        }
                        else
                            error = new Error { nError = 4, ErrorDescription = Resource.duplicateEmail };
                    }
                    else
                        error = new Error { nError = 4, ErrorDescription = Resource.msgFileSize };
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    error = new Error { nError = 4, ErrorDescription = Resource.invalidData };
                }
                    
            }
            catch(Exception ex)
            {
                SaveLog(ex, HttpContext);
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return Page();
        }

        public JsonResult OnPostCities(int countryId)
        {
            var listOCities = dbContext.Cities.Where(C => C.ContryId == countryId).Select(C => new SelectListItem { Value = C.Id.ToString(), Text = C.Name }).ToList();
            return new JsonResult(listOCities);
        }

        public JsonResult OnGetCheckIban(InputClass input)
        {
            return new JsonResult(ValidateIBAN(input.IBANCode));
        }

        public JsonResult OnGetCheckEmail(InputClass input)
        {
            if (dbContext.AspNetUsers.Where(U => U.Email == input.EmailAdress).Count() > 0)
                return new JsonResult(false);
            else 
                return new JsonResult(true);
        }

        public Error error { get; set; }

        [BindProperty]
        public InputClass Input { get; set; }

        public class InputClass
        {
            [Display(Name = "emri", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength:40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string FirstName { get; set; }

            [Display(Name = "mbiemri", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string LastName { get; set; }

            [Display(Name = "pershkrimiPozites", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string JobTitle { get; set; }

            [Display(Name = "gjinia", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int Gender { get; set; }

            [Display(Name = "nrTel", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "kontrolloFormatinTelefonit", ErrorMessageResourceType = typeof(Resource))]
            public string PhoneNumber { get; set; }

            [Display(Name = "emailAdresa", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.EmailAddress, ErrorMessageResourceName = "kontrolloFormatinEmail", ErrorMessageResourceType = typeof(Resource))]
            [PageRemote(PageHandler = "CheckEmail", HttpMethod = "GET", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "userExist")]
            public string EmailAdress { get; set; }

            [Display(Name = "adresa", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string Adress { get; set; }

            [Display(Name = "adresaOpsionale", ResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string AdressOpsional { get; set; }

            [Display(Name = "kodiPostal", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.PostalCode, ErrorMessageResourceName = "kontrolloFormatinKodiPostar", ErrorMessageResourceType = typeof(Resource))]
            [Range(1, 99999, ErrorMessageResourceName = "postalCodeError", ErrorMessageResourceType = typeof(Resource))]
            public int PostalCode { get; set; }

            [Display(Name = "qyteti", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int? City { get; set; }

            [Display(Name = "shteti", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int Contry { get; set; }

            [Display(Name = "ditaPunesimit", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string DtFrom { get; set; }

            [Display(Name = "dataPunesimitDeri", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string DtTo { get; set; }

            [Display(Name = "ditelindja", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string BirthDate { get; set; }

            [Display(Name = "ibanKode", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.CreditCard)]
            [PageRemote(PageHandler = "CheckIban",HttpMethod = "GET", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ibanValidation")]
            public string IBANCode { get; set; }

            [Display(Name = "pozitaPunes", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string Position { get; set; }

            [Display(Name = "kontrataPunes", ResourceType = typeof(Resource))]
            public IFormFile Contract { get; set; }
            
            [Display(Name = "numriPersonal", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessageResourceName = "stringLengthIDError", ErrorMessageResourceType = typeof(Resource))]
            public string PersonalNumber { get; set; }

            [Display(Name = "fotoProfilit", ResourceType = typeof(Resource))]
            public IFormFile UserProfileImg { get; set; }

            [Display(Name ="paga", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public float salary { get; set; }

            [Display(Name = "tatimi", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int TaxGroupId { get; set; }
        }
    }
}
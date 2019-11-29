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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESMS.Pages.Employees
{
    [Authorize(Policy = "CreateEmployee")]
    public class CreateModel : BaseModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CreateModel> _logger;
        private readonly IEmailSender _emailSender;

        public CreateModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<CreateModel> logger, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            error = new Error { };
            if (Input.Contract.Length / (1024 * 1024) <= 1)
            {
                if (!dbContext.AspNetUsers.Any(U => U.Email == Input.EmailAdress))
                {
                    var user = new ApplicationUser
                    {
                        Email = Input.EmailAdress,
                        Address = Input.Adress,
                        Address2 = Input.AdressOpsional,
                        BirthDate = Input.BirthDate,
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
                        EmploymentDate = Input.EmploymentDate,
                        PostCode = Input.PostalCode,
                        PhoneNumber = Input.PhoneNumber,
                        salary = Input.salary
                    };

                    var result = await _userManager.CreateAsync(user, Input.PersonalNumber);

                    if (!result.Succeeded)
                        error = new Error { nError = 4, ErrorDescription = "Ka ndodhur nje gabim gjate ruajtjes!" };
                    else
                    {
                        string role = dbContext.AspNetRoles.Where(R => R.Id == Input.Position).FirstOrDefault().Name;
                        await _userManager.AddToRoleAsync(user, role);
                        foreach (var claim in dbContext.AspNetRoleClaims.Where(R => R.Role.Id == Input.Position).ToList())
                            await _userManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue));

                        await _emailSender.SendEmailAsync("tonit.biba@hotmail.com", "Konfirmo llogarine", "Klikoni ne linkun e meposhtem per te konfirmuar llogarine tuaj!");


                        var pathOfSavedFile = SaveFiles(Input.Contract, FType.ContractFile);
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

                        if (Input.UserProfileImg != null)
                        {
                            var pathOfUserProfileImg = SaveFiles(Input.Contract, FType.GeneralFile);
                            dbContext.EmployeeDocuments.Add(new EmployeeDocuments
                            {
                                DtInserted = DateTime.Now,
                                NInsertedId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                                Employee = user.Id,
                                Name = Input.UserProfileImg.FileName,
                                Path = pathOfUserProfileImg,
                                Type = (int)FType.GeneralFile
                            });
                            await dbContext.SaveChangesAsync();
                        }
                        return RedirectToPage("List");
                    }
                }
                else
                    error = new Error { nError = 4, ErrorDescription = "Egziston perdorues me kete email adress." };
            }
            else
            {
                error = new Error { nError = 4, ErrorDescription = "Keni tejkaluar madhesine e fajllit." };
            }
            return Page();
        }

        public async Task<JsonResult> OnPostCities(int countryId)
        {
            var listOCities = dbContext.Cities.Where(C => C.ContryId == countryId).Select(C => new SelectListItem { Value = C.Id.ToString(), Text = C.Name }).ToList();
            return new JsonResult(listOCities);
        }

        public Error error { get; set; }

        [BindProperty]
        public InputClass Input { get; set; }
        public class InputClass
        {
            [Display(Name = "emri", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string FirstName { get; set; }

            [Display(Name = "mbiemri", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string LastName { get; set; }

            [Display(Name = "emriMesem", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string MiddleName { get; set; }

            [Display(Name = "pershkrimiPozites", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
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
            public string EmailAdress { get; set; }

            [Display(Name = "adresa", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string Adress { get; set; }

            [Display(Name = "adresaOpsionale", ResourceType = typeof(Resource))]
            public string AdressOpsional { get; set; }

            [Display(Name = "kodiPostal", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.PostalCode, ErrorMessageResourceName = "kontrolloFormatinKodiPostar", ErrorMessageResourceType = typeof(Resource))]
            public int PostalCode { get; set; }

            [Display(Name = "qyteti", ResourceType = typeof(Resource))]
            //[Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int? City { get; set; }

            [Display(Name = "shteti", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int Contry { get; set; }

            [Display(Name = "ditaPunesimit", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public DateTime EmploymentDate { get; set; }

            [Display(Name = "ditelindja", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            
            public DateTime BirthDate { get; set; }

            [Display(Name = "ibanKode", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.CreditCard)]
            public string IBANCode { get; set; }

            [Display(Name = "pozitaPunes", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string Position { get; set; }

            [Display(Name = "kontrataPunes", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public IFormFile Contract { get; set; }

            [Display(Name = "numriPersonal", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string PersonalNumber { get; set; }

            [Display(Name = "fotoProfilit", ResourceType = typeof(Resource))]
            public IFormFile UserProfileImg { get; set; }

            [Display(Name ="paga", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public float salary { get; set; }

        }
    }
}
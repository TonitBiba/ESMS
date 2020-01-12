using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace ESMS.Pages.Employees
{
    [Authorize(Policy = "Employee:Edit")]
    public class EditModel : BaseModel
    {
        public IConfiguration configuration { get; set; }
        private readonly IHubContext<NotificationHub> _hubContext;
        public UserManager<ApplicationUser> _userManager { get; set; }

        public EditModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHubContext<NotificationHub> hubContext) :base(signInManager, userManager) {
            this.configuration = configuration;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public void OnGet(string UIEnc)
        {
            string UserId = Confidenciality.Decrypt<string>(UIEnc);
            Input = dbContext.AspNetUsers.Where(U => U.Id == UserId).Select(U => new InputClass { 
                 UIEnc = UIEnc,
                 Adress = U.Address,
                 AdressOpsional = U.Address2,
                 BirthDate = U.BirthDate,
                 City = dbContext.Cities.Where(C=>C.Id == U.City).Select(S=>S.Name).FirstOrDefault(),
                 Contry = dbContext.Contries.Where(C => C.Id == U.Country).Select(S => S.Name).FirstOrDefault(),
                 EmailAdress = U.Email,
                 //EmploymentDate = U.EmploymentDate,
                 DtFrom = U.DtFrom,
                 DtTo = U.DtTo.ToString("yyyy-MM-dd"),
                 FirstName = U.FirstName,
                 Gender = U.Gender,
                 IBANCode = U.IbanCode,
                 JobTitle = U.JobTitle,
                 LastName = U.LastName,
                 PersonalNumber = U.PersonalNumber,
                 PhoneNumber = U.PhoneNumber,
                 salary = U.Salary,
                 PostalCode = (int)U.PostCode,
                 Position = U.AspNetUserRoles.FirstOrDefault().RoleId,
                 TaxGroupId = U.TaxGroupId
            }).FirstOrDefault();
        }

        public IActionResult OnGetDocument(string UIE, int docType)
        {
            string userId = Confidenciality.Decrypt<string>(UIE);
            if (docType == 1)
            {
                var filePath = dbContext.EmployeeDocuments.Where(u => u.Employee == userId).Select(S => new { S.Name, S.Path, S.DtInserted }).OrderByDescending(D => D.DtInserted).FirstOrDefault();
                var fileBytes = ShowFile(filePath.Path);
                return File(fileBytes, "application/pdf", filePath.Name);
            }
            else if(docType == 2)
            {
                var imgBytes = dbContext.AspNetUsers.Where(U => U.Id == userId).Select(U => U.UserProfile).FirstOrDefault();
                return File(imgBytes, "application/jpeg", "UserProfile.jpeg");
            }    
            return null;
        }

        public async Task<IActionResult> OnPost()
        {
            string userId = "";
            try
            {
                if (ModelState.IsValid)
                {
                    userId = Confidenciality.Decrypt<string>(Input.UIEnc);
                    var user = dbContext.AspNetUsers.Where(U => U.Id == userId).FirstOrDefault();
                    dbContext.AspNetUsersHistory.Add(new AspNetUsersHistory
                    {
                        Id = user.Id,
                        JobTitle = user.JobTitle,
                        LastName = user.LastName,
                        LockoutEnabled = user.LockoutEnabled,
                        LockoutEnd = user.LockoutEnd,
                        AccessFailedCount = user.AccessFailedCount,
                        NormalizedEmail = user.NormalizedEmail,
                        NormalizedUserName = user.NormalizedUserName,
                        Address = user.Address,
                        Address2 = user.Address2,
                        BirthDate = user.BirthDate,
                        City = user.City,
                        ConcurrencyStamp = user.ConcurrencyStamp,
                        Country = user.Country,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        EmployeeStatus = user.EmployeeStatus,
                        DtFrom = user.DtFrom,
                        DtTo = user.DtTo,
                        FirstName = user.FirstName,
                        Gender = user.Gender,
                        IbanCode = user.IbanCode,
                        PasswordHash = user.PasswordHash,
                        PersonalNumber = user.PersonalNumber,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        PostCode = user.PostCode,
                        Salary = user.Salary,
                        SecurityStamp = user.SecurityStamp,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        UserName = user.UserName,
                        UserProfile = user.UserProfile,
                        ChangePassword = user.ChangePassword,
                        Language = user.Language,
                        TaxGroupId = user.TaxGroupId
                    });
                    await dbContext.SaveChangesAsync();

                    byte[] userImages = null;
                    if (Input.UserProfileImg != null)
                    {
                        userImages = new byte[Input.UserProfileImg.Length];
                        BinaryReader imageBinary = new BinaryReader(Input.UserProfileImg.OpenReadStream());
                        userImages = imageBinary.ReadBytes((int)Input.UserProfileImg.Length);
                    }
                    user.JobTitle = Input.JobTitle;
                    user.Salary = Input.salary;
                    user.PostCode = Input.PostalCode;
                    user.Address = Input.Adress;
                    user.Address2 = Input.AdressOpsional;
                    user.PhoneNumber = Input.PhoneNumber;
                    user.IbanCode = Input.IBANCode;
                    user.UserProfile = userImages != null ? userImages : user.UserProfile;
                    user.TaxGroupId = Input.TaxGroupId;
                    user.DtTo = DateTime.ParseExact(Input.DtTo, "yyyy-MM-dd", null);

                    var applicationUser = await _userManager.FindByIdAsync(user.Id);
                    var roleId = await _userManager.GetRolesAsync(applicationUser);
                    if (roleId[0] != dbContext.AspNetRoles.Where(R => R.Id == Input.Position).FirstOrDefault().Name)
                    {
                        string currentBeAdded = roleId[0];
                        string RoleToBeAdded = dbContext.AspNetRoles.Where(R => R.Id == Input.Position).FirstOrDefault().Name;

                        foreach (var claim in dbContext.AspNetRoleClaims.ToList())
                            await _userManager.RemoveClaimAsync(applicationUser, new Claim(claim.ClaimType, claim.ClaimValue));

                        foreach (var claim in dbContext.AspNetRoleClaims.Where(R => R.Role.Id == Input.Position).ToList())
                            await _userManager.AddClaimAsync(applicationUser, new Claim(claim.ClaimType, claim.ClaimValue));

                        foreach (var role in dbContext.AspNetRoles.ToList())
                            await _userManager.RemoveFromRoleAsync(applicationUser, role.Name);

                        await _userManager.AddToRoleAsync(applicationUser, RoleToBeAdded);

                    }

                    if (Input.Contract != null)
                    {
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
                    }
                    dbContext.Notifications.Add(new Notifications
                    {
                        DtInserted = DateTime.Now,
                        Title = "Përditësim i të dhënave!",
                        VcIcon = "zmdi zmdi-edit",
                        VcInsertedUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        VcText = "Janë ndryshuar të dhënat në llogarinë tuaj nga përdoruesi: " + User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname),
                        VcUser = userId
                    });
                    await dbContext.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync(user.Id, "Janë ndryshuar të dhënat në llogarinë tuaj nga përdoruesi: " + User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname), "Janë përditësuar të dhënat.", "info", "/");
                }
                else
                {
                    error = new Error { nError = 4, ErrorDescription = Resource.invalidData};
                    return Page();
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex, HttpContext);
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
                return Page();
            }
            TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.perditesimiMeSukses });
            return RedirectToPage("List");
        }

        public JsonResult OnGetCheckIban(InputClass input)
        {
            return new JsonResult(ValidateIBAN(input.IBANCode));
        }

        public Error error { get; set; }

        [BindProperty]
        public InputClass Input { get; set; }

        public class InputClass
        {
            public string UIEnc { get; set; }

            [Display(Name = "emri", ResourceType = typeof(Resource))]
            public string FirstName { get; set; }

            [Display(Name = "mbiemri", ResourceType = typeof(Resource))]
            public string LastName { get; set; }

            [Display(Name = "emriMesem", ResourceType = typeof(Resource))]
            public string MiddleName { get; set; }

            [Display(Name = "pershkrimiPozites", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string JobTitle { get; set; }

            [Display(Name = "gjinia", ResourceType = typeof(Resource))]
            public int Gender { get; set; }

            [Display(Name = "nrTel", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "kontrolloFormatinTelefonit", ErrorMessageResourceType = typeof(Resource))]
            public string PhoneNumber { get; set; }

            [Display(Name = "emailAdresa", ResourceType = typeof(Resource))]
            public string EmailAdress { get; set; }

            [Display(Name = "adresa", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [StringLength(maximumLength: 40, MinimumLength = 1, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string Adress { get; set; }

            [Display(Name = "adresaOpsionale", ResourceType = typeof(Resource))]
            [Range(1, 40, ErrorMessageResourceName = "stringLengthError", ErrorMessageResourceType = typeof(Resource))]
            public string AdressOpsional { get; set; }

            [Display(Name = "kodiPostal", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.PostalCode, ErrorMessageResourceName = "kontrolloFormatinKodiPostar", ErrorMessageResourceType = typeof(Resource))]
            [Range(1, 99999, ErrorMessageResourceName = "postalCodeError", ErrorMessageResourceType = typeof(Resource))]
            public int PostalCode { get; set; }

            [Display(Name = "qyteti", ResourceType = typeof(Resource))]
            public string City { get; set; }

            [Display(Name = "shteti", ResourceType = typeof(Resource))]
            public string Contry { get; set; }

            [Display(Name = "ditaPunesimit", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public DateTime DtFrom { get; set; }

            [Display(Name = "dataPunesimitDeri", ResourceType = typeof(Resource))]
            public string DtTo { get; set; }

            [Display(Name = "ditelindja", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public DateTime BirthDate { get; set; }

            [Display(Name = "ibanKode", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            [DataType(DataType.CreditCard)]
            [PageRemote(PageHandler = "CheckIban", HttpMethod = "GET", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ibanValidation")]
            public string IBANCode { get; set; }

            [Display(Name = "pozitaPunes", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string Position { get; set; }

            [Display(Name = "kontrataPunes", ResourceType = typeof(Resource))]
            public IFormFile Contract { get; set; }

            [Display(Name = "numriPersonal", ResourceType = typeof(Resource))]
            public string PersonalNumber { get; set; }

            [Display(Name = "fotoProfilit", ResourceType = typeof(Resource))]
            public IFormFile UserProfileImg { get; set; }

            [Display(Name = "paga", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public float salary { get; set; }

            [Display(Name = "tatimi", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public int TaxGroupId { get; set; }
        }

    }
}
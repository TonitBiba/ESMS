﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Employees
{
    public class EditModel : BaseModel
    {
        public EditModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) :base(signInManager, userManager) { }

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
                 EmploymentDate = U.EmploymentDate,
                 FirstName = U.FirstName,
                 Gender = U.Gender,
                 IBANCode = U.IbanCode,
                 JobTitle = U.JobTitle,
                 LastName = U.LastName,
                 PersonalNumber = U.PersonalNumber,
                 PhoneNumber = U.PhoneNumber,
                 salary = U.Salary,
                 PostalCode = (int)U.PostCode,
                 Position = U.AspNetUserRoles.FirstOrDefault().RoleId
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
            try
            {
                string userId = Confidenciality.Decrypt<string>(Input.UIEnc);
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
                    EmploymentDate = user.EmploymentDate,
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
                    UserProfile = user.UserProfile
                });
                await dbContext.SaveChangesAsync();
                user.JobTitle = Input.JobTitle;
                user.Salary = Input.salary;
                user.PostCode = Input.PostalCode;
                user.Address = Input.Adress;
                user.Address2 = Input.AdressOpsional;
                user.PhoneNumber = Input.PhoneNumber;
                user.IbanCode = Input.IBANCode;
                await dbContext.SaveChangesAsync();
            }catch(Exception ex)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
                return Page();
            }
            TempData.Set<Error>("error", new Error { nError = 1, ErrorDescription = Resource.perditesimiMeSukses });
            return RedirectToPage("List");
        }

        public Error error { get; set; }

        [BindProperty]
        public InputClass Input { get; set; }

        public class InputClass
        {
            public string UIEnc { get; set; }

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
            public string City { get; set; }

            [Display(Name = "shteti", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public string Contry { get; set; }

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

            [Display(Name = "paga", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public float salary { get; set; }

        }

    }
}
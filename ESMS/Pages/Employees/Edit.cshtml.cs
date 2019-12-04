using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
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
                 Adress = U.Address,
                 AdressOpsional = U.Address2,
                 BirthDate = U.BirthDate,
                 City = U.City,
                 Contry = (int)U.City,
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
                 Position = "Developer"
            }).FirstOrDefault();
        }

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

            [Display(Name = "paga", ResourceType = typeof(Resource))]
            [Required(ErrorMessageResourceName = "fusheObligative", ErrorMessageResourceType = typeof(Resource))]
            public float salary { get; set; }

        }

    }
}
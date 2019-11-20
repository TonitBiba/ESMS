using ESMS.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ESMS.Pages.Employees
{
    [Authorize(Policy = "CreateEmployee")]
    public class CreateModel : BaseModel
    {
        public void OnGet()
        {

        }

        public class InputClass
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string JobTitle { get; set; }
            public int Gender { get; set; }
            public string PhoneNumber { get; set; }
            public string EmailAdress { get; set; }
            public string Adress { get; set; }
            public string AdressOpsional { get; set; }
            public int PostalCode { get; set; }
            public int City { get; set; }
            public int Contry { get; set; }
            public DateTime EmploymentDate { get; set; }
            public int EmployeementStatus { get; set; }
            public DateTime BirthDate { get; set; }
            public string PersonalNumber { get; set; }
            public string IBANCode { get; set; }



        }

    }
}
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [MaxLength(128)]
        public string JobTitle { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public int? PostCode { get; set; }

        public int? City { get; set; }

        public int? Country { get; set; }

        public DateTime DtFrom { get; set; }

        public DateTime DtTo { get; set; }

        public int EmployeeStatus { get; set; }

        [Required]
        [MaxLength(32)]
        public string IbanCode { get; set; }

        [Required]
        [MaxLength(12)]
        public string PersonalNumber { get; set; }

        [Required]
        public float salary { get; set; }

        public byte[] UserProfile { get; set; }

        public int TaxGroupId { get; set; }

        public int? language { get; set; }

    }
}

﻿using ESMS.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESMS.General_Classes
{
    public class CustomClaismAuthentication : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public CustomClaismAuthentication(
    UserManager<ApplicationUser> userManager
    , RoleManager<IdentityRole> roleManager
    , IOptions<IdentityOptions> optionsAccessor)
: base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {new Claim(ClaimTypes.GivenName, user.FirstName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.Surname, user.LastName),
                });
            }

            if (user.language != null)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.Locality, user.language.ToString()),
                });
            }


            return principal;
        }
    }
}

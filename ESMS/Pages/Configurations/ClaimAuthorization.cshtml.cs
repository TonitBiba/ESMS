using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Data.Model;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Configurations
{
    public class ClaimAuthorizationModel : BaseModel
    {
        public void OnGet()
        {
            string groupId = null;
            var listOfClaims = dbContext.Policy.Select(P => new ListOfClaims
            {
                nPolicyId = P.NPolicyId,
                vcPolicyName = P.VcPolicyName,
                vcClaimType = P.VcClaimType,
                vcClaimValue = P.VcClaimType,
                vcAccess = dbContext.AspNetRoleClaims.Any(R => R.RoleId == groupId && R.ClaimType == P.VcClaimType)
            }).ToList();
        }

        public PartialViewResult OnGetPartialUpdate(string groupId)
        {
            var listOfClaims = dbContext.Policy.Select(P => new ListOfClaims
            {
                nPolicyId = P.NPolicyId,
                vcPolicyName = P.VcPolicyName,
                vcClaimType = P.VcClaimType,
                vcClaimValue = P.VcClaimType,
                vcAccess = dbContext.AspNetRoleClaims.Any(R => R.RoleId == groupId && R.ClaimType == P.VcClaimType)
            }).ToList();
            return Partial("ClaimAuthorization", listOfClaims);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Configurations
{
    public class _ListClaimsAuthorizationModel : BaseModel
    {


        public void OnGet(string groupId)
        {
            listOfClaims = dbContext.Policy.Select(P => new ListOfClaims
            {
                nPolicyId = P.NPolicyId,
                vcPolicyName = P.VcPolicyName,
                vcClaimType = P.VcClaimType,
                vcClaimValue = P.VcClaimType,
                vcAccess = dbContext.AspNetRoleClaims.Any(R => R.RoleId == groupId && R.ClaimType == P.VcClaimType)
            }).ToList();
        }

        public async Task<JsonResult> OnPostChangePermission(string groupId, string PEnc)
        {
            Error error = new Error { nError = 1, ErrorDescription = Resource.msgRuajtjaSukses };
            try
            {
                int policyId = Confidenciality.Decrypt<int>(PEnc);
                if (dbContext.AspNetRoleClaims.Any(T => T.RoleId == groupId && T.ClaimType == dbContext.Policy.Where(P => P.NPolicyId == policyId).Select(P => P.VcClaimType).FirstOrDefault()))
                {
                    dbContext.AspNetRoleClaims.Remove(dbContext.AspNetRoleClaims.Where(R => R.RoleId == groupId && R.ClaimType == dbContext.Policy.Where(P => P.NPolicyId == policyId).Select(P => P.VcClaimType).FirstOrDefault()).FirstOrDefault());
                    dbContext.AspNetUserClaims.RemoveRange(dbContext.AspNetUserClaims.Where(C => C.ClaimType== dbContext.Policy.Where(P => P.NPolicyId == policyId).Select(P => P.VcClaimType).FirstOrDefault() && C.User.AspNetUserRoles.Where(R => R.RoleId == groupId).FirstOrDefault().RoleId==groupId));
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    var policy = dbContext.Policy.Where(P => P.NPolicyId == policyId).FirstOrDefault();
                    dbContext.AspNetRoleClaims.Add(new AspNetRoleClaims
                    {
                        ClaimType = policy.VcClaimType,
                        ClaimValue = policy.VcClaimValue,
                        RoleId = groupId
                    });

                    var users = dbContext.AspNetUserRoles.Where(R => R.RoleId == groupId).Select(R => R.User);
                    
                    foreach (var userToChange in users)
                    {
                        userToChange.AspNetUserClaims.Add(new AspNetUserClaims {
                            ClaimType = policy.VcClaimType,
                            ClaimValue = policy.VcClaimValue
                        });
                    }

                    await dbContext.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return new JsonResult(error);
        }

        public IList<ListOfClaims> listOfClaims { get; set; }

    }
}
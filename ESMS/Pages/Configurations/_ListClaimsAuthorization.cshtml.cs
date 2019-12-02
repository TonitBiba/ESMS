using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Pages.Shared;
using ESMS.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace ESMS.Pages.Configurations
{
    public class _ListClaimsAuthorizationModel : BaseModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHubContext<NotificationHub> _hubContext;

       public _ListClaimsAuthorizationModel
            (SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext):base(signInManager, userManager)
       {
            _signInManager = signInManager;
           _userManager = userManager;
            _hubContext = hubContext;
       }

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
            bool access = false;
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
                    access = true;
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
                }
                await dbContext.SaveChangesAsync();
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                await _signInManager.RefreshSignInAsync(user);
                string policyName = dbContext.Policy.Where(P => P.NPolicyId == policyId).Select(S => S.VcPolicyName).FirstOrDefault();
                List<Notifications> notifications = dbContext.AspNetUserRoles.Where(UR => UR.RoleId == groupId).Select(R => new Notifications 
                { 
                     DtInserted = DateTime.Now,
                     Title = access?"Eshte shtuar qasja":"Eshte larguar qasja",
                     VcIcon = access? "zmdi zmdi-lock-open" : "zmdi zmdi-lock",
                      VcInsertedUser = User.FindFirstValue(ClaimTypes.NameIdentifier),
                     VcUser = R.UserId,
                     VcText = "Eshte " + (access?"shtuar":"larguar") + " qasja per "+ policyName
                }).ToList();

                if (notifications.Count() > 0)
                {
                    dbContext.Notifications.AddRange(notifications);
                    await dbContext.SaveChangesAsync();
                }

                await _hubContext.Clients.All.SendAsync(groupId, (access ? "Eshte shtuar qasja" : "Eshte larguar qasja")+ " për " + policyName, "Qasja!", "info", "/");
            }
            catch(Exception ex)
            {
                error = new Error { nError = 4, ErrorDescription = Resource.msgGabimRuajtja };
            }
            return new JsonResult(error);
        }

        public IList<ListOfClaims> listOfClaims { get; set; }

    }
}
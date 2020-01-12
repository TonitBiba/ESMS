using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ESMS.Areas.Identity;
using ESMS.Data.Model;
using ESMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace ESMS.Pages.Shared
{
    [Authorize]
    public class BaseModel : PageModel
    {
        protected SignInManager<ApplicationUser> signInManager;

        protected UserManager<ApplicationUser> userManager;
        
        public static byte[] userProfile;

        protected ESMSContext dbContext=null;

        public BaseModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            dbContext = new ESMSContext();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var user = dbContext.AspNetUsers.Where(U => U.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(U => new { U.UserProfile, U.ChangePassword }).FirstOrDefault();
            userProfile = user.UserProfile;
            signInManager.RefreshSignInAsync(userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result).ConfigureAwait(false);
            if(user.ChangePassword == true)
            {
                context.HttpContext.Response.Redirect("/Identity/Account/Manage/ChangePassword");
            }
            dbContext.Logs.Add(new Logs
            {
              DtInserted = DateTime.Now,
              Hostname = context.HttpContext.Connection.RemoteIpAddress.ToString(),
              IpAdress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
              Method = context.HttpContext.Request.Method,
              Page = context.ActionDescriptor.RouteValues.Values.FirstOrDefault().ToString(),
              StatusCode = context.HttpContext.Response.StatusCode,
              Url = context.ActionDescriptor.RelativePath,
              UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });
            dbContext.SaveChanges();
        }

        public void SaveLog(Exception ex, HttpContext context)
        {
            using (ESMSContext esms = new ESMSContext())
            {
                esms.Logs.Add(new Logs
                {
                    BError = true,
                    DtInserted = DateTime.Now,
                    Exception = ex.Message,
                    Hostname = context.Connection.RemoteIpAddress.ToString(),
                    IpAdress = context.Connection.RemoteIpAddress.ToString(),
                    Method = context.Request.Method,
                    Page = context.Request.Path.Value,
                    StatusCode = context.Response.StatusCode,
                    Url = context.Request.Path.Value,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    ExceptionDetail = ex.StackTrace
                });
                esms.SaveChanges();
            }
        }

        #region DropDownList
        public static List<SelectListItem> GetGroups(int nCase, string UID)
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                switch (nCase)
                {
                    case 1:
                        return dbContext.AspNetRoles.Select(R => new SelectListItem { Text = R.Name, Value = R.Id }).ToList();
                    case 2:
                        string[] userGroupd = null;
                        if (UID == "Administrator")
                            userGroupd = new string[] { "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e", "dbc05ab9-f41f-493f-b3e6-689d14e88dda", "be007199-39b1-4557-b10f-cc4e6dc47b49", "423a5ce2-3024-47d1-b486-4dcd3951871b", "58cfa2e9-9eeb-4fcd-b87c-6d0b676fb066" };
                        else if(UID == "Menagjer_IT")
                            userGroupd = new string[] { "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e"};
                        else if (UID == "Menagjer_Financa")
                            userGroupd = new string[] { "dbc05ab9-f41f-493f-b3e6-689d14e88dda" };
                        else if (UID == "Burimet_Njerzore")
                            userGroupd = new string[] { "a15cae60-f564-4b36-9c60-5cb9d7eb7f1e", "dbc05ab9-f41f-493f-b3e6-689d14e88dda", "be007199-39b1-4557-b10f-cc4e6dc47b49", "423a5ce2-3024-47d1-b486-4dcd3951871b", "58cfa2e9-9eeb-4fcd-b87c-6d0b676fb066" };
                        return dbContext.AspNetRoles.Where(R=> userGroupd.Contains(R.Id)).Select(R => new SelectListItem { Text = R.Name, Value = R.Id }).ToList();
                }
                return null;
            }
        }

        public static List<SelectListItem> GetGenders()
        {
            return new List<SelectListItem> { new SelectListItem { Text = Resource.mashkull, Value = "1" }, new SelectListItem { Text = Resource.femer, Value = "2" }};
        }

        public static List<SelectListItem> GetContries()
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                List<SelectListItem> countries = new List<SelectListItem> { new SelectListItem { Value = "247", Text = "Kosovë" }};
                countries.AddRange(dbContext.Contries.Where(R=>R.Id!=247).Select(R => new SelectListItem { Text = R.Name, Value = R.Id.ToString() }).ToList());
                return countries;
            }
        }

        public static List<SelectListItem> GetMonths(int language)
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                var usedMonths = dbContext.Payments.Where(P => P.DtInserted.Year == DateTime.Now.Year).Select(s => s.Month).ToList();

                return dbContext.Month.Where(M=> !usedMonths.Contains(M.Id)).Select(M=>new SelectListItem { Text = language == 1?M.MonthSq:M.MonthEn, Value = M.Id.ToString() }).ToList();
            }
        }

        public static List<SelectListItem> GetLeaveTypes(int language)
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                return dbContext.TypeOfLeaves.Select(M => new SelectListItem { Text = language == 1 ? M.NameSq : M.NameEn, Value = M.Id.ToString() }).ToList();
            }
        }

        public static List<SelectListItem> GetTaxGroups(int language)
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                return dbContext.TaxGroup.Select(M => new SelectListItem { Text = language == 1 ? M.Name : M.NameEng, Value = M.Id.ToString() }).ToList();
            }
        }

        public static List<SelectListItem> GetStatuses(int NCase, int language)
        {
            using (ESMSContext dbContext = new ESMSContext())
            {
                int[] nStatuses = new int[] { 2, 3, 4 };
                return dbContext.Status.Where(s=> nStatuses.Contains(s.Id)).Select(M => new SelectListItem { Text = language == 1 ? M.NameSq : M.NameEn, Value = M.Id.ToString() }).ToList();
            }
        }

        public static List<SelectListItem> GetEmployeeStatus()
        {
            return new List<SelectListItem> { new SelectListItem { Value = "1", Text = Resource.aktiv }, 
                                              new SelectListItem { Value = "0", Text = Resource.pasiv }
                                               };
        }

        public static List<SelectListItem> GetReportTypes()
        {
            return new List<SelectListItem> {
                    new SelectListItem { Text = Resource.employers, Value = "1" },
                    new SelectListItem { Text = Resource.payment, Value = "2"},
                    new SelectListItem { Text = Resource.requestLeave, Value = "3"}
            };
        }

        public static List<SelectListItem> GetUsers()
        {
            using (ESMSContext dbContext = new ESMSContext())
            {        
                 return dbContext.AspNetUsers.Where(U=>U.AspNetUserRoles.FirstOrDefault().RoleId != "2a13875f-53af-45a5-b240-48a90ff993a5").Select(R => new SelectListItem { Text = R.FirstName+" "+R.LastName, Value = R.Id.ToString()}).ToList();
            }
        }

        #endregion

        public string getFormatReport(int format)
        {
            string strFormat = "";
            switch (format)
            {
                case 1:
                    strFormat = "PDF";
                    break;
                case 2:
                    strFormat = "Excel";
                    break;
                case 3:
                    strFormat = "Word";
                    break;
                case 4:
                    strFormat = "PowerPoint";
                    break;
            }
            return strFormat;
        }

        public string getExtension(int format)
        {
            string extension = "";
            switch (format)
            {
                case 1:
                    extension = ".pdf";
                    break;
                case 2:
                    extension = ".xls";
                    break;
                case 3:
                    extension = ".doc";
                    break;
                default:
                    break;
            }
            return extension;
        }

        protected string SaveFiles(IFormFile file, FType fileType, IConfiguration configuration)
        {
            if(file == null)
                return null;

            string path = "";
            if (fileType == FType.ContractFile)
                path = configuration.GetSection("AppSettings").GetSection("ContractFiles").Value;
            else if (fileType == FType.GeneralFile)
                path = configuration.GetSection("AppSettings").GetSection("GeneralFiles").Value;
            else if (fileType == FType.AnnualLeaveFile)
            {
                path = configuration.GetSection("AppSettings").GetSection("AnnualLeaveFile").Value;
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) + ".zip";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            using (var stream = file.OpenReadStream())
            using (var zsp = new GZipStream(fs, CompressionMode.Compress))
                stream.CopyTo(zsp);
            fs.Close();

            Confidenciality.EncryptFile(path, path+".enc");

            System.IO.File.Delete(path);

            return path+".enc";
        }

        protected byte[] ShowFile(string path)
        {
            var decryptedFile = Confidenciality.DecryptFile(path);
            var decompresedFile = Decompress(decryptedFile);
            return decompresedFile;
        }

        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static bool ValidateIBAN(string ibanValue)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ibanValue, "^[A-Z0-9]"))
            {
                ibanValue = ibanValue.Replace(" ", String.Empty);
                string iban =
                ibanValue.Substring(4, ibanValue.Length - 4) + ibanValue.Substring(0, 4);
                int asciiShift = 55;
                StringBuilder sb = new StringBuilder();
                foreach (char c in iban)
                {
                    int v;
                    if (Char.IsLetter(c))
                    {
                        v = c - asciiShift;
                    }
                    else
                    {
                        v = int.Parse(c.ToString());
                    }
                    sb.Append(v);
                }
                string checkSumString = sb.ToString();
                int checksum = int.Parse(checkSumString.Substring(0, 1));
                for (int i = 1; i < checkSumString.Length; i++)
                {
                    int v = int.Parse(checkSumString.Substring(i, 1));
                    checksum *= 10;
                    checksum += v;
                    checksum %= 97;
                }
                return checksum == 1;
            }
            else
            {
                return false;
            }
        }

        protected enum FType
        {
            ContractFile = 1,
            GeneralFile = 2,
            AnnualLeaveFile = 3
        }

        protected enum Statuses
        {
            Applied = 1,
            Completed = 5
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.Net;

namespace ESMS.Pages.Employees
{
    public class ReportModel : PageModel
    {

        public void OnGet()
        {
            
            Download_SSRSInPDF();

        }

        public IActionResult OnGetShkarkoRaportin()
        {
            byte[] reportBytes = null;
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;

                reportBytes = client.DownloadData("http://tonit/ReportServer/Pages/ReportViewer.aspx?%2fESSMSReportServer%2fReport1&rs:Format=PDF");
            }

            return File(reportBytes, "application/pdf");
        }

        public void Download_SSRSInPDF()
        {

        }
    }
}
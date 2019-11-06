using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESMS.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESMS.Pages.Configurations
{
    public class _MenuListModel : PageModel
    {
        ESMSContext ESMSContext = new ESMSContext();
        public void OnGetAsync()
        {
            menus =  ESMSContext.Menu.ToList(); 
        }

        public IList<Menu> menus { get; set; }

        public async Task<JsonResult> deleteMenu()
        {
            try
            {
                
            }
            catch
            {

            }
            return new JsonResult(null);
        }


    }
}
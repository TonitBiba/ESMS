using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class SubMenu
    {
        public SubMenu()
        {
            GroupMenu = new HashSet<GroupMenu>();
        }

        public int NSubMenuId { get; set; }
        public int NMenuId { get; set; }
        public string VcSubMenuSq { get; set; }
        public string VcSubMenuEn { get; set; }
        public string VcController { get; set; }
        public string VcPage { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public DateTime? DtModify { get; set; }

        public virtual AspNetUsers NInserted { get; set; }
        public virtual Menu NMenu { get; set; }
        public virtual ICollection<GroupMenu> GroupMenu { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class GroupMenu
    {
        public int NGroupMenyId { get; set; }
        public string VcAspNetRolesId { get; set; }
        public int NSubMenuId { get; set; }
        public bool BAccess { get; set; }
        public int NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public int? NModifyId { get; set; }
        public DateTime? DtModify { get; set; }

        public virtual SubMenu NSubMenu { get; set; }
        public virtual AspNetRoles VcAspNetRoles { get; set; }
    }
}

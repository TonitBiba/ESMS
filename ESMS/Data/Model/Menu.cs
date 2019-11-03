using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Menu
    {
        public Menu()
        {
            SubMenu = new HashSet<SubMenu>();
        }

        public int NMenuId { get; set; }
        public string VcMenNameSq { get; set; }
        public string VcMenuNameEn { get; set; }
        public string VcIcon { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public string NModifyId { get; set; }
        public DateTime? DtModify { get; set; }

        public virtual AspNetUsers NInserted { get; set; }
        public virtual ICollection<SubMenu> SubMenu { get; set; }
    }
}

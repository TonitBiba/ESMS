using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Leaves
    {
        public Leaves()
        {
            LeavesDetails = new HashSet<LeavesDetails>();
        }

        public int Id { get; set; }
        public int NTypeOfLeaves { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VcComment { get; set; }
        public string VcDocumentPath { get; set; }
        public DateTime DtInserted { get; set; }
        public string VcUser { get; set; }

        public virtual TypeOfLeaves NTypeOfLeavesNavigation { get; set; }
        public virtual AspNetUsers VcUserNavigation { get; set; }
        public virtual ICollection<LeavesDetails> LeavesDetails { get; set; }
    }
}

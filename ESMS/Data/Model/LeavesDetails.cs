using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class LeavesDetails
    {
        public int Id { get; set; }
        public int NLeaves { get; set; }
        public int NStatus { get; set; }
        public string VcComment { get; set; }
        public bool BActive { get; set; }
        public string VcUser { get; set; }
        public DateTime DtInserted { get; set; }

        public virtual Leaves NLeavesNavigation { get; set; }
        public virtual Status NStatusNavigation { get; set; }
        public virtual AspNetUsers VcUserNavigation { get; set; }
    }
}

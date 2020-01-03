using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Status
    {
        public Status()
        {
            LeavesDetails = new HashSet<LeavesDetails>();
        }

        public int Id { get; set; }
        public string NameSq { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<LeavesDetails> LeavesDetails { get; set; }
    }
}

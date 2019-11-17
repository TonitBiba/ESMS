using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Policy
    {
        public int NPolicyId { get; set; }
        public string VcPolicyName { get; set; }
        public string VcClaimType { get; set; }
        public string VcClaimValue { get; set; }
        public bool BActive { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public string NModifyId { get; set; }
        public DateTime? DtModify { get; set; }
    }
}

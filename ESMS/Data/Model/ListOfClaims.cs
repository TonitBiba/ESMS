using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESMS.Data.Model
{
    public class ListOfClaims
    {
        public int nPolicyId { get; set; }
        public string vcPolicyName { get; set; }
        public string vcClaimType { get; set; }
        public string vcClaimValue { get; set; }
        public bool vcAccess { get; set; }
    }
}

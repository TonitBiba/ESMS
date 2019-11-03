using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Logs
    {
        public long NLogId { get; set; }
        public string IpAdress { get; set; }
        public string Hostname { get; set; }
        public string UserId { get; set; }
        public DateTime? DtInserted { get; set; }
        public string Page { get; set; }
        public string Method { get; set; }
    }
}

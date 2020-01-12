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
        public string Url { get; set; }
        public int? StatusCode { get; set; }
        public bool? BError { get; set; }
        public string Exception { get; set; }
        public string ExceptionDetail { get; set; }
    }
}

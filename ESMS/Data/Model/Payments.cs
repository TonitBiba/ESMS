using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Payments
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Month { get; set; }
        public decimal Salary { get; set; }
        public DateTime DtInserted { get; set; }
        public string VcInserted { get; set; }

        public virtual Month MonthNavigation { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual AspNetUsers VcInsertedNavigation { get; set; }
    }
}

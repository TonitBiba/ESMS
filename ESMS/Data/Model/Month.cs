using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Month
    {
        public Month()
        {
            Payments = new HashSet<Payments>();
        }

        public int Id { get; set; }
        public string MonthSq { get; set; }
        public string MonthEn { get; set; }

        public virtual ICollection<Payments> Payments { get; set; }
    }
}

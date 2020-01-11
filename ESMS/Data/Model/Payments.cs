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
        public decimal? Deduction { get; set; }
        public decimal? SalaryAfterDeduction { get; set; }
        public decimal? EmployeePension { get; set; }
        public decimal? EmployerPension { get; set; }
        public decimal? TaxableIncome { get; set; }
        public decimal? WithholdingTax { get; set; }
        public decimal? NetWage { get; set; }

        public virtual Month MonthNavigation { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual AspNetUsers VcInsertedNavigation { get; set; }
    }
}

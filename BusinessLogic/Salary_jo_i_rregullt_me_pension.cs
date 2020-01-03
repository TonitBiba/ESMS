using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
   public   class Salary_jo_i_rregullt_me_pension
    {

        private const double Level3 = (450 - 250) * 0.08;
        private const double Level2 = (250 - 80) * 0.04;
        public const double PesionTaxRate = 5.00;
        public double TotalSalary_me_tatim;
        public double EmployerPension { get; set; }
         public double TotalSalary { get; set; }
        public double TaxedAmount { get; set; }

        public Salary_jo_i_rregullt_me_pension(double salary)
        {
            TotalSalary = salary;
            CalculateEmployerPension_jo_i_rregullt_me_pension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension_jo_i_rregullt_me_pension();
        }
        public Salary_jo_i_rregullt_me_pension(double salary,int hours,int price)
        {
            TotalSalary = salary + (hours * price);
            CalculateEmployerPension_jo_i_rregullt_me_pension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension_jo_i_rregullt_me_pension();
        }
        public Salary_jo_i_rregullt_me_pension(double salary, int hours, int price, int overhours, int priceoverhours)
        {
            TotalSalary = salary + (hours * price) + (overhours * priceoverhours);
            CalculateEmployerPension_jo_i_rregullt_me_pension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension_jo_i_rregullt_me_pension();
        }
        public Salary_jo_i_rregullt_me_pension(int hours,int pricehours)
        {
            TotalSalary = hours * pricehours;
            CalculateEmployerPension_jo_i_rregullt_me_pension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension_jo_i_rregullt_me_pension();
        }
        public Salary_jo_i_rregullt_me_pension(int hours, int pricehours, int overhours, int priceoverhours)
        {
            TotalSalary = (hours*pricehours)+(overhours*priceoverhours);
            CalculateEmployerPension_jo_i_rregullt_me_pension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension_jo_i_rregullt_me_pension();
        }
        public double CalculateEmployerPension_jo_i_rregullt_me_pension()
        {
            EmployerPension = ((TotalSalary * PesionTaxRate) / 100.00);
            return EmployerPension;
        }
        public double Te_ardhurat_e_tatueshme_jo_i_rregullt_me_pension()
        {
            return TotalSalary - CalculateEmployerPension_jo_i_rregullt_me_pension();
        }
        public double CalculateTax_jo_i_rregullt_me_pension()
        {

            return Te_ardhurat_e_tatueshme_jo_i_rregullt_me_pension() * 0.1;
        }

        public double Pagat_e_paguara_pas_tatimit_jo_i_rreggult_me_pension()
        {

            double rezultati = Te_ardhurat_e_tatueshme_jo_i_rregullt_me_pension() - CalculateTax_jo_i_rregullt_me_pension(); 
            return rezultati;



        }



    }
}

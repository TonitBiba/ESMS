using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public  class Salary_i_rregullt_pa_pension
     {
        public double TotalSalary_me_tatim;
        private const double Level3 = (450 - 250) * 0.08;
        private const double Level2 = (250 - 80) * 0.04;
        public const double PesionTaxRate = 5.00;
        public double TaxedAmount { get; set; }

        public Salary_i_rregullt_pa_pension(double salary)
        {
           TotalSalary_me_tatim = salary;
       
        }
        public Salary_i_rregullt_pa_pension(double salary,int hours,int price)
        {
            TotalSalary_me_tatim = salary + (hours * price);
            CalculateEmployerPension_i_rregullt_pa_pension();
            TotalSalary_me_tatim = TotalSalary_me_tatim - CalculateEmployerPension_i_rregullt_pa_pension();
        }
        public Salary_i_rregullt_pa_pension(double salary, int hours, int price, int overhours, int priceoverhours)

        {
            TotalSalary_me_tatim = salary + (hours * price) + (overhours * priceoverhours);
            CalculateEmployerPension_i_rregullt_pa_pension();
            TotalSalary_me_tatim = TotalSalary_me_tatim - CalculateEmployerPension_i_rregullt_pa_pension();
        }

        public Salary_i_rregullt_pa_pension(int hours,int pricehours)
        {
            TotalSalary_me_tatim = hours * pricehours;
            CalculateEmployerPension_i_rregullt_pa_pension();
            TotalSalary_me_tatim = TotalSalary_me_tatim - CalculateEmployerPension_i_rregullt_pa_pension();
        }
        public Salary_i_rregullt_pa_pension(int hours, int pricehours, int overhours, int priceoverhours)
        {
            TotalSalary_me_tatim = (hours * pricehours) + (overhours * priceoverhours);
            CalculateEmployerPension_i_rregullt_pa_pension();
            TotalSalary_me_tatim = TotalSalary_me_tatim - CalculateEmployerPension_i_rregullt_pa_pension();
        }
       public double CalculateEmployerPension_i_rregullt_pa_pension()
       {
           return 0;
       }

       public double Te_ardhurat_e_tatueshme_i_rregullt_pa_pension()
       {
           return TotalSalary_me_tatim;
       }

       public double CalculateTax_i_rregult_pa_pension()
       {
           if (TotalSalary_me_tatim > 80)
           {
               if (TotalSalary_me_tatim > 250)
               {
                   if (TotalSalary_me_tatim > 450)
                   {
                       var taxamount1 = (TotalSalary_me_tatim - 450) * 0.10;
                       TaxedAmount = taxamount1 + Level3 + Level2;
                   }
                   else
                   {
                       var taxamount = (TotalSalary_me_tatim - 250) * 0.08;
                       TaxedAmount = taxamount + Level2;
                   }
               }
               else
               {
                   var taxamount = TotalSalary_me_tatim - 80;
                   TaxedAmount = taxamount * 0.04;
               }
           }
           else
           {
               TaxedAmount = 0;
           }
           return TaxedAmount;
       }


       public double Pagat_e_paguara_pas_tatimit_i_rregullt_pa_pension()
       {
           CalculateTax_i_rregult_pa_pension();
           double rezultati = TotalSalary_me_tatim - TaxedAmount;
           return rezultati;

       }

    }
}

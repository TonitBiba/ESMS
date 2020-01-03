namespace BusinessLogic
{
    public class Salary
    {
        private const double Level3 = (450 - 250)*0.08;
        private const double Level2 = (250 - 80)*0.04;
        public const double PesionTaxRate = 5.00;
        public double TotalSalary_me_tatim;

        public Salary(double salary)
        {
             TotalSalary = salary;
             CalculateEmployerPension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension();
        }
        public Salary(double salary,int hours,int price)
         {
           TotalSalary = salary + (hours * price);
           CalculateEmployerPension();
           TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension();
         }
        public Salary(double salary, int hours, int price, int overhours, int priceoverhours)
        {
            TotalSalary = salary + (hours * price) + (overhours * priceoverhours);
            CalculateEmployerPension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension();
        }

        public Salary(int hours,int pricehours)

        {
            TotalSalary = hours * pricehours;
            CalculateEmployerPension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension();
        }

        public Salary(int hours,int pricehours,int overhours,int priceoverhours)

        {
            TotalSalary = (hours*pricehours)+(overhours*priceoverhours);
            CalculateEmployerPension();
            TotalSalary_me_tatim = TotalSalary - CalculateEmployerPension();
        }

        public double TotalSalary { get; set; }
        public double EmployerPension { get; set; }
        public double TaxedAmount { get; set; }

        public double CalculateSalary()

        {
            return TotalSalary*1.1;
        }

        public  double  CalculateEmployerPension()
        {
            EmployerPension = ((TotalSalary*PesionTaxRate)/100.00);
            return EmployerPension;
        }

        public double CalculateTax()
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
                    TaxedAmount = taxamount*0.04;
               }
            }

            else
            {
                TaxedAmount = 0;
            }
            return TaxedAmount;
         }
        public double Te_ardhurat_e_tatueshme()
        {
            return TotalSalary - CalculateEmployerPension();
        }

          public double Pagat_e_paguara_pas_tatimit()
          {
            CalculateTax();
            double rezultati = Te_ardhurat_e_tatueshme()-TaxedAmount;
            return rezultati;
          }
    }
}
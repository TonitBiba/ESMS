using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
   public static  class CalculateSalaryGeneral
    {
        public static double CalculateEmployeePension(decimal salaryBruto, int taxgroupId)
        {

            double salary = Convert.ToDouble(salaryBruto);

            switch (taxgroupId)
            {
                case 1:
                    Salary s = new Salary(salary);
                    return s.CalculateEmployerPension();
                    
                case 2:
                    Salary_i_rregullt_pa_pension b = new Salary_i_rregullt_pa_pension(salary);
                    return b.CalculateEmployerPension_i_rregullt_pa_pension();
                case 3:
                    Salary_jo_i_rregullt_me_pension c = new Salary_jo_i_rregullt_me_pension(salary);
                    return c.CalculateEmployerPension_jo_i_rregullt_me_pension();

                default:
                    Salary_jo_i_rregullt_pa_pension d = new Salary_jo_i_rregullt_pa_pension(salary);
                    return d.CalculateEmployerPension_jo_i_rregullt_pa_pension();
                   
            }

        }

        public static double CalculateTaxableIncome(decimal salaryBruto, int taxgroupId)
        {

            double salary = Convert.ToDouble(salaryBruto);

            switch (taxgroupId)
            {
                case 1:
                    Salary s = new Salary(salary);
                    return s.Te_ardhurat_e_tatueshme();

                case 2:
                    Salary_i_rregullt_pa_pension b = new Salary_i_rregullt_pa_pension(salary);
                    return b.Te_ardhurat_e_tatueshme_i_rregullt_pa_pension();
                case 3:
                    Salary_jo_i_rregullt_me_pension c = new Salary_jo_i_rregullt_me_pension(salary);
                    return c.Te_ardhurat_e_tatueshme_jo_i_rregullt_me_pension();

                default:
                    Salary_jo_i_rregullt_pa_pension d = new Salary_jo_i_rregullt_pa_pension(salary);
                    return d.Te_ardhurat_e_tatueshme_jo_i_rregullt_pa_pension();

            }

        }

        public static double CalculateWithHoldingTax(decimal salaryBruto, int taxgroupId)
        {

            double salary = Convert.ToDouble(salaryBruto);

            switch (taxgroupId)
            {
                case 1:
                    Salary s = new Salary(salary);
                    return s.CalculateTax();

                case 2:
                    Salary_i_rregullt_pa_pension b = new Salary_i_rregullt_pa_pension(salary);
                    return b.CalculateTax_i_rregult_pa_pension();
                case 3:
                    Salary_jo_i_rregullt_me_pension c = new Salary_jo_i_rregullt_me_pension(salary);
                    return c.CalculateTax_jo_i_rregullt_me_pension();

                default:
                    Salary_jo_i_rregullt_pa_pension d = new Salary_jo_i_rregullt_pa_pension(salary);
                    return d.CalculateTax_jo_i_rregullt_pa_pension();

            }

        }

        public static double CalculateNetWage(decimal salaryBruto, int taxgroupId)
        {

            double salary = Convert.ToDouble(salaryBruto);

            switch (taxgroupId)
            {
                case 1:
                    Salary s = new Salary(salary);
                    return s.Pagat_e_paguara_pas_tatimit();

                case 2:
                    Salary_i_rregullt_pa_pension b = new Salary_i_rregullt_pa_pension(salary);
                    return b.Pagat_e_paguara_pas_tatimit_i_rregullt_pa_pension();
                case 3:
                    Salary_jo_i_rregullt_me_pension c = new Salary_jo_i_rregullt_me_pension(salary);
                    return c.Pagat_e_paguara_pas_tatimit_jo_i_rreggult_me_pension();

                default:
                    Salary_jo_i_rregullt_pa_pension d = new Salary_jo_i_rregullt_pa_pension(salary);
                    return d.Pagat_e_paguara_pas_tatimit_jo_i_rreggult_pa_pension();

            }

        }




    }
}

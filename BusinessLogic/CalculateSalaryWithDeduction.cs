using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public static class CalculateSalaryWithDeduction
    {
        public static decimal CalculateSalaryWithDed(float SalaryBeforeDeduct, int deductionpercentage)
        {
            decimal FinalSalary = 0;

            decimal decution = (deductionpercentage * Convert.ToDecimal(SalaryBeforeDeduct)) / 100;

            FinalSalary = Convert.ToDecimal(SalaryBeforeDeduct)- decution;

            return FinalSalary;


        }

    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Data
{
    public class EmployeePayrollDeduction
    {
        public int EmployeePayrollId { get; set; }

        [ForeignKey(nameof(EmployeePayrollId))]
        public EmployeePayroll? EmployeePayroll { get; set; }


        public int EmployeeDeductionId { get; set; }

        [ForeignKey(nameof(EmployeeDeductionId))]
        public EmployeeDeduction? EmployeeDeduction { get; set; }


        public decimal Amount { get; set; }
    }
}

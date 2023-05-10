using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Data
{
    public class EmployeePayrollAllowance
    {
        public int EmployeePayrollId { get; set; }

        [ForeignKey(nameof(EmployeePayrollId))]
        public EmployeePayroll? EmployeePayroll { get; set; }


        public int EmployeeAllowanceId { get; set; }

        [ForeignKey(nameof(EmployeeAllowanceId))]
        public EmployeeAllowance? EmployeeAllowance { get; set; }


        public decimal Amount { get; set; } 
    }
}

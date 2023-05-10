using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Data
{
    public class EmployeePayroll
    {
        public int Id { get; set; } 

        public int PayrollId { get; set; }

        [ForeignKey(nameof(PayrollId))]
        public Payroll? Payroll { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public decimal BasicSalary { get; set; }    

        public decimal AllowanceAmount { get; set; }

        public decimal DeductionAmount { get; set; }

        public decimal NetAmount { get; set; }  //  calculated 


        public virtual ICollection<EmployeePayrollAllowance> PayrollAllowances { get;set; }
        public virtual ICollection<EmployeePayrollDeduction> PayrollDeductions { get; set; }

    }
}

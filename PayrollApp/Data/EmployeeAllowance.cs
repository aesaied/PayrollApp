using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Data
{
    public class EmployeeAllowance
    {

        public int Id { get; set; } 

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }


        public int AllowanceId { get; set; }
        [ForeignKey(nameof(AllowanceId))]
        public Allowance? Allowance { get; set; }

        [Range(1, 5000)]
        public decimal Amount { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool IsActive { get; set; }  


        public virtual ICollection<EmployeePayrollAllowance> PayrollAllowances { get;set; }


    
    }
}

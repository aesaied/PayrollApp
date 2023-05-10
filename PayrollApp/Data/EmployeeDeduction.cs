using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Data
{
    public class EmployeeDeduction
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }


        public int DeductionId { get; set; }
        [ForeignKey(nameof(DeductionId))]
        public Deduction? Deduction { get; set; }

        [Range(1, 5000)]
        public decimal Amount { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool IsActive { get; set; }


        public virtual ICollection<EmployeePayrollDeduction>  PayrollDeductions { get; set;}
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Data
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string? EmpNo { get; set; }

        [Required]
        [StringLength(9)]
        public string? IDNo { get; set; }

        [Required]
        [StringLength(20)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string? SecondName { get; set; }

        [Required]
        [StringLength(20)]
        public string? ThirdName { get; set; }

        [Required]
        [StringLength(20)]
        public string?   LastName { get;set; }


        public string? FullName { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }


        public int PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public Position? Position { get; set; }

        public Gender Gender { get; set; }

        [Range(1000, 25000)]
       
        public decimal BasicSalary { get; set; }    


        public virtual ICollection<EmployeeAllowance> Allowances { get; set; }
        public virtual ICollection<EmployeeDeduction> Deductions { get; set; }



    }
}

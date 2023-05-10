using PayrollApp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Models
{
    public class CreateOrEditEmployee
    {
        public int? Id { get; set; }

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
        public string? LastName { get; set; }



        public int DepartmentId { get; set; }

       


        public int PositionId { get; set; }

       

        public Gender Gender { get; set; }

        [Range(1000, 25000)]

        public decimal BasicSalary { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Data
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }


    }
}

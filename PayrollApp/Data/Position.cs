using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Data
{
    public class Position
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }


        public virtual ICollection<Employee> Employees { get; set; }
    }
}

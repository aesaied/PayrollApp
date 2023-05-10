using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Data
{
    public class Payroll
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Description { get; set; }

       public int Month { get; set; }

        public int Year { get; set; }
    }
}

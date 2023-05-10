using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Data
{
    public class Deduction
    {
        public int Id { get; set; }


        [Required]
        [StringLength(100)]
        public string? Description { get; set; }

        [Range(1, 5000)]
        public decimal DefaultAmount { get; set; }

      

    }
}

using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Models
{
    public class CreateUserInfo
    {
        public string? Id { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(  DataType.Password)]
        public string Password { get; set; }
        [Required]

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CofirmPassword { get; set; }

    }
}

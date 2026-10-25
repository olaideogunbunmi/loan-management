using System.ComponentModel.DataAnnotations;

namespace LoanManagement.Dtos
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}

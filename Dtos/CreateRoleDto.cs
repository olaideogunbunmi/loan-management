using System.ComponentModel.DataAnnotations;

namespace LoanManagement.Dtos
{
    public class CreateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

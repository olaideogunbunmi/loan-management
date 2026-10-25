using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Dtos
{
    public class CreateLoanDto
    {
        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Range(3, 12, ErrorMessage = "Tenor must be minimum of 3 months and maximum of 12 months")]
        public byte TenorMonth { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal InterestRate { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string CustomerEmail { get; set; } = string.Empty;
    }
}

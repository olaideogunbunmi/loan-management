using LoanManagement.Models;

namespace LoanManagement.Dtos
{
    public class LoanDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; }
        public decimal InterestRate { get; set; }
        public decimal LoanAmount { get; set; }
        public byte TenorMonth { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagement.Models
{
    public class Loan
    {
        public Guid LoanId { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal LoanAmount { get; set; }
        public byte TenorMonth { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal InterestRate { get; set; }
        public LoanStatus Status { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public Guid SalesOfficerId {  get; set; }
    }

    public enum LoanStatus
    {
        Pending,
        Rejected,
        Captured,
        Approved
    }
}

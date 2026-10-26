namespace LoanManagement.Dtos
{
    public class ReviewResponseDto
    {
        public bool Success { get; set; }
        public int RejectedLoanCount { get; set; }
        public int CapturedLoanCount { get; set; }
    }
}

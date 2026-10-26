using LoanManagement.Dtos;
using LoanManagement.Models;

namespace LoanManagement.Interfaces
{
    public interface ILoanService
    {
        Task<CreateResponseDto> CreateAsync(CreateLoanDto dto, string email);
        Task<ReviewResponseDto> ReviewAsync();
        Task<CaptureResponseDto> CaptureAsync();
        Task<List<LoanDto>> GetLoansAsync();
    }
}

using LoanManagement.Dtos;

namespace LoanManagement.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(CreateUserDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}

using LoanManagement.Dtos;

namespace LoanManagement.Interfaces
{
    public interface IRoleService
    {
        Task<CreateResponseDto> CreateAsync(CreateRoleDto dto);

    }
}

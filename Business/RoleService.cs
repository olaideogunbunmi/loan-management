using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Interfaces;
using LoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Business
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CreateResponseDto> CreateAsync(CreateRoleDto dto)
        {
            var role = new Role()
            {
                RoleName = dto.Name
            };

            await _context.AddAsync(role);
            await _context.SaveChangesAsync();

            return new CreateResponseDto()
            {
                Success = true
            };

        }
    }
}

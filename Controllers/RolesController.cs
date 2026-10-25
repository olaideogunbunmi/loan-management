using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleDto dto)
        {
            var role = new Role()
            {
                RoleName = dto.Name
            };

            await _context.AddAsync(role);
            await _context.SaveChangesAsync();

            return Created();
        }
    }
}

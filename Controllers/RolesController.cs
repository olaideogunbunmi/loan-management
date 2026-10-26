using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Interfaces;
using LoanManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleDto dto)
        {
            var result = await _roleService.CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest();
            }


            return Created();
        }
    }
}

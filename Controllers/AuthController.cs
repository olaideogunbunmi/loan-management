using LoanManagement.Dtos;
using LoanManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LoanManagement.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(CreateUserDto dto)
        {
           var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
            {
                return NotFound("Role not found");
            }

            return Created();
        }

        [HttpPost("login")]
        public async Task<ActionResult> login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized();
            }

            return Ok(new { Token = result.Token });
        }
    }
}

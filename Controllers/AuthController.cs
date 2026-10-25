using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanManagement.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(CreateUserDto dto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == dto.Role);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            var user = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                RoleId = role.RoleId
            };            

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPost("login")]
        public async Task<ActionResult> login(LoginDto dto)
        {
            var user = await _context.Users.Where(x => x.Email == dto.Email && x.Password == dto.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }

            var role = await _context.Roles.Where(x => x.RoleId == user.RoleId).Select(x => x.RoleName).FirstOrDefaultAsync();

            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var secretKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:7113",
                audience: "https://localhost:7113",
                claims: new List<Claim>()
                {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role)
                },
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString });
        }
    }
}

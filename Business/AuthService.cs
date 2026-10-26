using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Interfaces;
using LoanManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanManagement.Business
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<RegisterResponseDto> RegisterAsync(CreateUserDto dto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == dto.Role);

            if (role == null)
            {
                return new RegisterResponseDto()
                {
                    Success = false
                };
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

            return new RegisterResponseDto()
            {
                Success = true
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.Where(x => x.Email == dto.Email && x.Password == dto.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Success = false
                };
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

            return new LoginResponseDto()
            {           
                Success = true,
                Token = tokenString
            };
        }
    }
}

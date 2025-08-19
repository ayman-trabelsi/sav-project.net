using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;
using MiniProjet.Context;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Shared.ModelsDto;

namespace MiniProjet.Repository
{
    public class AuthentificationRepository : IAuthentificationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthentificationRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserDto> ValidateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Roles = new List<string> { user.Role }  // Assuming single role as string
            };
        }

        public async Task<string> LoginAsync(LoginUserDto loginUserDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);
                if (user == null)
                {
                    Console.WriteLine($"Login failed: User not found with email {loginUserDto.Email}");
                    return null;
                }

                if (!VerifyPassword(loginUserDto.Password, user.PasswordHash))
                {
                    Console.WriteLine($"Login failed: Invalid password for user {loginUserDto.Email}");
                    return null;
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role ?? "Client")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:TokenExpiryMinutes")),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                Console.WriteLine($"Login successful for user {user.Email}");
                return tokenString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoginAsync error: {ex}");
                throw;
            }
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto dto)
        {
            try
            {
                if (await UserExistsAsync(dto.Email, dto.Name))
                    return false;

                var user = new User
                {
                    Email = dto.Email,
                    Username = dto.Name,
                    PasswordHash = HashPassword(dto.Password),
                    Role = "Client", // Or "Admin" if needed
                    UserType = "Client" // Ensure UserType is set to Client
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RegisterUserAsync: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            return await _context.Users.AnyAsync(u => u.Email == email || u.Username == username);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }
    }
}

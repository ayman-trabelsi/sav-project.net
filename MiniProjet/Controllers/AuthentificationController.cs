using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniProjet.ModelsDto;
using MiniProjet.Repository.IRepository;
using Shared.ModelsDto;
using Microsoft.Extensions.Logging;

namespace MiniProjet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentificationRepository _authRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthentificationRepository authRepository, IOptions<JwtSettings> jwtSettings, ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Registration attempt with null data");
                    return BadRequest("Registration data is required");
                }

                if (string.IsNullOrWhiteSpace(dto.Email))
                {
                    _logger.LogWarning("Registration attempt with empty email");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    _logger.LogWarning("Registration attempt with empty name");
                    return BadRequest("Name is required");
                }

                if (string.IsNullOrWhiteSpace(dto.Password))
                {
                    _logger.LogWarning("Registration attempt with empty password");
                    return BadRequest("Password is required");
                }

                _logger.LogInformation("Checking if user exists with email {Email} or name {Name}", dto.Email, dto.Name);
                if (await _authRepository.UserExistsAsync(dto.Email, dto.Name))
                {
                    _logger.LogWarning("Registration failed: Email {Email} or name {Name} already exists", dto.Email, dto.Name);
                    return BadRequest("Email or username is already in use.");
                }

                _logger.LogInformation("Registering new user with email {Email}", dto.Email);
                var success = await _authRepository.RegisterUserAsync(dto);
                if (!success)
                {
                    _logger.LogError("Failed to register user with email {Email}", dto.Email);
                    return StatusCode(500, "Error during registration.");
                }

                _logger.LogInformation("Successfully registered user with email {Email}", dto.Email);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email {Email}", dto?.Email);
                return StatusCode(500, "An error occurred during registration");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            try
            {
                if (loginDto == null)
                {
                    _logger.LogWarning("Login attempt with null data");
                    return BadRequest("Login data is required");
                }

                if (string.IsNullOrWhiteSpace(loginDto.Email))
                {
                    _logger.LogWarning("Login attempt with empty email");
                    return BadRequest("Email is required");
                }

                if (string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    _logger.LogWarning("Login attempt with empty password");
                    return BadRequest("Password is required");
                }

                _logger.LogInformation("Attempting login for user with email {Email}", loginDto.Email);
                var token = await _authRepository.LoginAsync(loginDto);
                if (token == null)
                {
                    _logger.LogWarning("Login failed for user with email {Email}: Invalid credentials", loginDto.Email);
                    return Unauthorized("Invalid email or password");
                }

                _logger.LogInformation("Successfully logged in user with email {Email}", loginDto.Email);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email {Email}", loginDto?.Email);
                return StatusCode(500, "An error occurred during login");
            }
        }
    }
}

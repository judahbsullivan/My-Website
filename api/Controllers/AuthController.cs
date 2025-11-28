using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Api.Services;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });
            if (
                string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password)
            )
                return BadRequest(new { message = "Email and Password are required." });

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName ?? string.Empty,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(new { 
                    message = string.Join(" ", result.Errors.Select(e => e.Description)),
                    errors = result.Errors.Select(e => e.Description)
                });

            var token = _tokenService.CreateToken(user.Id, user.Email);
            return Ok(
                new
                {
                    token,
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.DisplayName,
                    },
                }
            );
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });
            if (
                string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password)
            )
                return BadRequest(new { message = "Email and Password are required." });

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!validPassword)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = _tokenService.CreateToken(user.Id, user.Email ?? string.Empty);
            return Ok(
                new
                {
                    token,
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.DisplayName,
                    },
                }
            );
        }
    }

    public class RegisterRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DisplayName { get; set; }
    }

    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Api.Services;
using MyPortfolio.Data;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly AppDbContext _context;

        public AuthController(UserManager<AppUser> userManager, TokenService tokenService, EmailService emailService, AppDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _context = context;
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

            // Normalize email to lowercase for consistent lookup
            var normalizedEmail = request.Email?.Trim().ToLowerInvariant();
            var user = await _userManager.FindByEmailAsync(normalizedEmail ?? string.Empty);
            if (user == null)
            {
#if DEBUG
                Console.WriteLine($"Login failed: User not found for email {request.Email}");
#endif
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!validPassword)
            {
#if DEBUG
                Console.WriteLine($"Login failed: Invalid password for user {user.Email} (ID: {user.Id})");
#endif
                return Unauthorized(new { message = "Invalid credentials" });
            }

#if DEBUG
            Console.WriteLine($"Login successful for user {user.Email} (ID: {user.Id})");
#endif

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

        // POST: api/auth/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { message = "Email is required." });

            // Normalize email to lowercase for consistent lookup
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var user = await _userManager.FindByEmailAsync(normalizedEmail);
            if (user == null)
            {
                // Don't reveal if user exists for security
                return Ok(new { message = "If an account exists, a password reset link has been sent." });
            }

            // Generate password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // Send password reset email
            await _emailService.SendPasswordResetEmailAsync(user.Email ?? string.Empty, user.Id, resetToken);
            
            // Never return the token in the response for security
            return Ok(new
            {
                message = "If an account exists with this email, a password reset link has been sent."
            });
        }

        // GET: api/auth/validate-reset-token
        // Validates if a reset token is valid before showing the reset form
        [HttpGet("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string userId, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest(new { message = "UserId and Token are required.", isValid = false });

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return Ok(new { message = "Invalid user or reset token.", isValid = false });

                // URL decode the token in case it was encoded
                var decodedToken = Uri.UnescapeDataString(token);

                // Verify the token is valid (without actually resetting the password)
                // We use VerifyUserTokenAsync to check if the token is valid
                var isValid = await _userManager.VerifyUserTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    UserManager<AppUser>.ResetPasswordTokenPurpose,
                    decodedToken
                );

                if (!isValid)
                    return Ok(new { message = "Invalid or expired reset token.", isValid = false });

                return Ok(new { message = "Token is valid.", isValid = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating reset token: {ex.Message}");
                return Ok(new { message = "Invalid or expired reset token.", isValid = false });
            }
        }

        // POST: api/auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Request body is required." });
                if (string.IsNullOrWhiteSpace(request.UserId) || 
                    string.IsNullOrWhiteSpace(request.Token) || 
                    string.IsNullOrWhiteSpace(request.NewPassword))
                    return BadRequest(new { message = "UserId, Token, and NewPassword are required." });

                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    return BadRequest(new { message = "Invalid user or reset token." });

                // URL decode the token in case it was encoded
                var decodedToken = Uri.UnescapeDataString(request.Token);

                // Verify token is still valid before attempting reset
                var tokenIsValid = await _userManager.VerifyUserTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    UserManager<AppUser>.ResetPasswordTokenPurpose,
                    decodedToken
                );

                if (!tokenIsValid)
                {
                    return BadRequest(new
                    {
                        message = "The reset token is invalid or has expired. Please request a new password reset link.",
                        errors = new[] { "Invalid or expired token" }
                    });
                }

                // Reset the password - this will:
                // 1. Validate the token
                // 2. Hash the new password
                // 3. Save the new password hash to the database
                // 4. Invalidate the token (one-time use)
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
                
                if (!result.Succeeded)
                {
                    var errorMessages = result.Errors.Select(e => e.Description).ToList();
                    var mainMessage = errorMessages.Any(e => e.Contains("Invalid token") || e.Contains("expired"))
                        ? "The reset token is invalid or has expired. Please request a new password reset link."
                        : string.Join(" ", errorMessages);
                    
#if DEBUG
                    Console.WriteLine($"Password reset failed for user {user.Email}: {string.Join(", ", errorMessages)}");
#endif
                    
                    return BadRequest(new
                    {
                        message = mainMessage,
                        errors = errorMessages
                    });
                }

                // Reload the user from the database to ensure we have the latest password hash
                // This is important because the entity might be cached
                await _context.Entry(user).ReloadAsync();
                
                // Also get a fresh instance from UserManager
                var refreshedUser = await _userManager.FindByIdAsync(user.Id);
                if (refreshedUser == null)
                {
#if DEBUG
                    Console.WriteLine($"ERROR: Could not reload user {user.Id} after password reset");
#endif
                    return StatusCode(500, new { 
                        message = "Password was reset but user could not be verified. Please try logging in." 
                    });
                }

                // Verify the password was actually saved by checking it with the refreshed user
                var passwordCheck = await _userManager.CheckPasswordAsync(refreshedUser, request.NewPassword);
                if (!passwordCheck)
                {
#if DEBUG
                    Console.WriteLine($"WARNING: Password reset succeeded but password verification failed for user {refreshedUser.Email}");
                    Console.WriteLine($"Password hash exists: {!string.IsNullOrEmpty(refreshedUser.PasswordHash)}");
#endif
                    return StatusCode(500, new { 
                        message = "Password was reset but verification failed. Please try logging in, or request a new reset link if login fails." 
                    });
                }

#if DEBUG
                Console.WriteLine($"Password successfully reset for user {refreshedUser.Email} (ID: {refreshedUser.Id})");
                Console.WriteLine($"Password hash updated: {!string.IsNullOrEmpty(refreshedUser.PasswordHash)}");
#endif
                
                // Password has been successfully reset and saved to the database
                // The token is now invalidated (one-time use)
                return Ok(new { message = "Password has been reset successfully. You can now login with your new password." });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
#if DEBUG
                Console.WriteLine($"Error resetting password: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
#else
                _ = ex; // Suppress unused variable warning in release
#endif
                
                return StatusCode(500, new { 
                    message = "An error occurred while resetting your password. Please try again or request a new reset link." 
                });
            }
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

    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }
}

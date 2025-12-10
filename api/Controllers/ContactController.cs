using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Api.Services;
using MyPortfolio.Data;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public ContactController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // POST: api/contact
        [HttpPost]
        public async Task<ActionResult> SendMessage([FromBody] ContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Name is required" });
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { message = "Email is required" });
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { message = "Message is required" });

            // Input validation: length checks
            if (request.Name.Length > 100)
                return BadRequest(new { message = "Name must be 100 characters or less" });
            if (request.Email.Length > 200)
                return BadRequest(new { message = "Email must be 200 characters or less" });
            if (request.Subject != null && request.Subject.Length > 100)
                return BadRequest(new { message = "Subject must be 100 characters or less" });
            if (request.Message.Length > 5000)
                return BadRequest(new { message = "Message must be 5000 characters or less" });

            // Basic email format validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                return BadRequest(new { message = "Invalid email format" });

            var message = new ContactMessage
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                Subject = request.Subject?.Trim() ?? string.Empty,
                Message = request.Message.Trim()
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            // Send email notification
            try
            {
                await _emailService.SendContactFormEmailAsync(
                    message.Name,
                    message.Email,
                    string.IsNullOrWhiteSpace(message.Subject) ? "No Subject" : message.Subject,
                    message.Message
                );
            }
            catch (Exception ex)
            {
                // Error sending email - message is already saved, so don't fail the request
            }

            return Ok(new { message = "Message sent successfully" });
        }
    }

    public class ContactRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}


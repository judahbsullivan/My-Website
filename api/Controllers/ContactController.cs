using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Data;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
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

            var message = new ContactMessage
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                Subject = request.Subject?.Trim() ?? string.Empty,
                Message = request.Message.Trim()
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            // TODO: Send email notification here if needed

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


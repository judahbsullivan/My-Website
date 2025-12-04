using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Models;
using System.Security.Claims;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaygroundController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PlaygroundController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Helper method to check if current user has active subscription
        private async Task<bool> HasActiveSubscriptionAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return false;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            // Check if subscription is active and not expired
            if (!user.HasActiveSubscription)
                return false;

            if (user.SubscriptionExpiresAt.HasValue && user.SubscriptionExpiresAt.Value < DateTime.UtcNow)
                return false;

            return true;
        }

        // GET: api/playground
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaygroundDto>>> GetPlaygrounds()
        {
            var hasSubscription = await HasActiveSubscriptionAsync();

            var playgrounds = await _context.Playgrounds
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.PublishedAt)
                .Select(p => new PlaygroundDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Description = p.Description,
                    Category = p.Category,
                    CodeType = p.CodeType,
                    PreviewImageUrl = p.PreviewImageUrl,
                    IsVueComponent = p.IsVueComponent,
                    IsInteractive = p.IsInteractive,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    IsFeatured = p.IsFeatured,
                    RequiresSubscription = p.RequiresSubscription,
                    PublishedAt = p.PublishedAt
                })
                .ToListAsync();

            return Ok(playgrounds);
        }

        // GET: api/playground/featured
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<PlaygroundDto>>> GetFeaturedPlaygrounds([FromQuery] int count = 6)
        {
            var playgrounds = await _context.Playgrounds
                .Where(p => p.IsPublished && p.IsFeatured)
                .OrderByDescending(p => p.PublishedAt)
                .Take(count)
                .Select(p => new PlaygroundDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Description = p.Description,
                    Category = p.Category,
                    CodeType = p.CodeType,
                    PreviewImageUrl = p.PreviewImageUrl,
                    IsVueComponent = p.IsVueComponent,
                    IsInteractive = p.IsInteractive,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    IsFeatured = p.IsFeatured,
                    RequiresSubscription = p.RequiresSubscription,
                    PublishedAt = p.PublishedAt
                })
                .ToListAsync();

            return Ok(playgrounds);
        }

        // GET: api/playground/{slug}
        [HttpGet("{slug}")]
        public async Task<ActionResult<PlaygroundDetailDto>> GetPlayground(string slug)
        {
            var playground = await _context.Playgrounds
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

            if (playground == null)
                return NotFound(new { message = "Playground item not found" });

            // Increment view count
            playground.ViewCount++;
            await _context.SaveChangesAsync();

            var hasSubscription = await HasActiveSubscriptionAsync();
            var isLocked = playground.RequiresSubscription && !hasSubscription;

            var dto = new PlaygroundDetailDto
            {
                Id = playground.Id,
                Slug = playground.Slug,
                Title = playground.Title,
                Description = playground.Description,
                Category = playground.Category,
                Code = isLocked ? string.Empty : playground.Code, // Hide code if locked
                CodeType = playground.CodeType,
                Css = isLocked ? null : playground.Css, // Hide CSS if locked
                JavaScript = isLocked ? null : playground.JavaScript, // Hide JS if locked
                Dependencies = isLocked ? new List<string>() : (JsonSerializer.Deserialize<List<string>>(playground.Dependencies) ?? new List<string>()),
                Tags = JsonSerializer.Deserialize<List<string>>(playground.Tags) ?? new List<string>(),
                PreviewImageUrl = playground.PreviewImageUrl,
                IsVueComponent = playground.IsVueComponent,
                IsInteractive = playground.IsInteractive,
                ViewCount = playground.ViewCount,
                LikeCount = playground.LikeCount,
                RequiresSubscription = playground.RequiresSubscription,
                IsLocked = isLocked, // Indicate if content is locked
                PublishedAt = playground.PublishedAt,
                Author = playground.Author != null ? new AuthorDto
                {
                    Id = playground.Author.Id,
                    DisplayName = playground.Author.DisplayName,
                    Email = playground.Author.Email ?? string.Empty
                } : null
            };

            return Ok(dto);
        }

        // POST: api/playground (Admin only)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Playground>> CreatePlayground([FromBody] CreatePlaygroundRequest request)
        {
            if (await _context.Playgrounds.AnyAsync(p => p.Slug == request.Slug))
                return BadRequest(new { message = "A playground item with this slug already exists" });

            var playground = new Playground
            {
                Slug = request.Slug,
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                Code = request.Code,
                CodeType = request.CodeType ?? "vue",
                Css = request.Css,
                JavaScript = request.JavaScript,
                Dependencies = JsonSerializer.Serialize(request.Dependencies ?? new List<string>()),
                Tags = JsonSerializer.Serialize(request.Tags ?? new List<string>()),
                PreviewImageUrl = request.PreviewImageUrl,
                IsVueComponent = request.IsVueComponent ?? false,
                IsInteractive = request.IsInteractive ?? false,
                IsFeatured = request.IsFeatured,
                IsPublished = request.IsPublished,
                RequiresSubscription = request.RequiresSubscription ?? false,
                PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
                AuthorId = request.AuthorId
            };

            _context.Playgrounds.Add(playground);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayground), new { slug = playground.Slug }, playground);
        }
    }

    // DTOs
    public class PlaygroundDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CodeType { get; set; } = string.Empty;
        public string? PreviewImageUrl { get; set; }
        public bool IsVueComponent { get; set; }
        public bool IsInteractive { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsFeatured { get; set; }
        public bool RequiresSubscription { get; set; }
        public DateTime? PublishedAt { get; set; }
    }

    public class PlaygroundDetailDto : PlaygroundDto
    {
        public string Code { get; set; } = string.Empty;
        public string? Css { get; set; }
        public string? JavaScript { get; set; }
        public List<string> Dependencies { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public bool IsLocked { get; set; } // Indicates if content is locked due to subscription
        public AuthorDto? Author { get; set; }
    }

    public class CreatePlaygroundRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? CodeType { get; set; } = "vue";
        public string? Css { get; set; }
        public string? JavaScript { get; set; }
        public List<string>? Dependencies { get; set; }
        public List<string>? Tags { get; set; }
        public string? PreviewImageUrl { get; set; }
        public bool? IsVueComponent { get; set; }
        public bool? IsInteractive { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPublished { get; set; }
        public bool? RequiresSubscription { get; set; }
        public string? AuthorId { get; set; }
    }
}


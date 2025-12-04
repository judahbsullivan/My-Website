using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPosts()
        {
            var posts = await _context.BlogPosts
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.PublishedAt)
                .Select(p => new BlogPostDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Excerpt = p.Excerpt,
                    Image = p.Image,
                    ReadTimeMinutes = p.ReadTimeMinutes,
                    IsFeatured = p.IsFeatured,
                    PublishedAt = p.PublishedAt
                })
                .ToListAsync();

            return Ok(posts);
        }

        // GET: api/blog/latest
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetLatestPosts([FromQuery] int count = 3)
        {
            var posts = await _context.BlogPosts
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.PublishedAt)
                .Take(count)
                .Select(p => new BlogPostDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Excerpt = p.Excerpt,
                    Image = p.Image,
                    ReadTimeMinutes = p.ReadTimeMinutes,
                    IsFeatured = p.IsFeatured,
                    PublishedAt = p.PublishedAt
                })
                .ToListAsync();

            return Ok(posts);
        }

        // GET: api/blog/{slug}
        [HttpGet("{slug}")]
        public async Task<ActionResult<BlogPostDetailDto>> GetPost(string slug)
        {
            var post = await _context.BlogPosts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

            if (post == null)
                return NotFound(new { message = "Blog post not found" });

            var dto = new BlogPostDetailDto
            {
                Id = post.Id,
                Slug = post.Slug,
                Title = post.Title,
                Category = post.Category,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Image = post.Image,
                Tags = JsonSerializer.Deserialize<List<string>>(post.Tags) ?? new List<string>(),
                ReadTimeMinutes = post.ReadTimeMinutes,
                PublishedAt = post.PublishedAt,
                Author = post.Author != null ? new AuthorDto
                {
                    Id = post.Author.Id,
                    DisplayName = post.Author.DisplayName,
                    Email = post.Author.Email ?? string.Empty
                } : null
            };

            return Ok(dto);
        }

        // GET: api/blog/{slug}/related
        [HttpGet("{slug}/related")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetRelatedPosts(string slug)
        {
            var currentPost = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Slug == slug);
            if (currentPost == null)
                return NotFound();

            var related = await _context.BlogPosts
                .Where(p => p.IsPublished && p.Slug != slug && p.Category == currentPost.Category)
                .OrderByDescending(p => p.PublishedAt)
                .Take(3)
                .Select(p => new BlogPostDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Excerpt = p.Excerpt,
                    Image = p.Image,
                    ReadTimeMinutes = p.ReadTimeMinutes,
                    PublishedAt = p.PublishedAt
                })
                .ToListAsync();

            return Ok(related);
        }

        // POST: api/blog (Admin only)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BlogPost>> CreatePost([FromBody] CreateBlogPostRequest request)
        {
            if (await _context.BlogPosts.AnyAsync(p => p.Slug == request.Slug))
                return BadRequest(new { message = "A post with this slug already exists" });

            var post = new BlogPost
            {
                Slug = request.Slug,
                Title = request.Title,
                Category = request.Category,
                Excerpt = request.Excerpt,
                Content = request.Content,
                Image = request.Image,
                Tags = JsonSerializer.Serialize(request.Tags ?? new List<string>()),
                ReadTimeMinutes = request.ReadTimeMinutes,
                IsFeatured = request.IsFeatured,
                IsPublished = request.IsPublished,
                PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
                AuthorId = request.AuthorId
            };

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { slug = post.Slug }, post);
        }
    }

    // DTOs
    public class BlogPostDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int ReadTimeMinutes { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? PublishedAt { get; set; }
    }

    public class BlogPostDetailDto : BlogPostDto
    {
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public AuthorDto? Author { get; set; }
    }

    public class AuthorDto
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CreateBlogPostRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Image { get; set; }
        public List<string>? Tags { get; set; }
        public int ReadTimeMinutes { get; set; } = 5;
        public bool IsFeatured { get; set; }
        public bool IsPublished { get; set; }
        public string? AuthorId { get; set; }
    }
}


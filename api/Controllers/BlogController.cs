using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Supabase;
using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Postgrest;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public BlogController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // GET: api/blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPosts()
        {
            try
            {
                var response = await _supabase
                    .From<BlogPostSupabase>()
                    .Where(x => x.IsPublished == true)
                    .Order(x => x.IsFeatured, Constants.Ordering.Descending)
                    .Order(x => x.PublishedAt, Constants.Ordering.Descending)
                    .Get();

                var posts = response.Models.Select(p => new BlogPostDto
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
                }).ToList();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching blog posts",
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        // GET: api/blog/latest
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetLatestPosts([FromQuery] int count = 3)
        {
            try
            {
                var response = await _supabase
                    .From<BlogPostSupabase>()
                    .Where(x => x.IsPublished == true)
                    .Order(x => x.PublishedAt, Constants.Ordering.Descending)
                    .Limit(count)
                    .Get();

                var posts = response.Models.Select(p => new BlogPostDto
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
                }).ToList();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching latest blog posts",
                    error = ex.Message
                });
            }
        }

        // GET: api/blog/{slug}
        [HttpGet("{slug}")]
        public async Task<ActionResult<BlogPostDetailDto>> GetPost(string slug)
        {
            try
            {
                var post = await _supabase
                    .From<BlogPostSupabase>()
                    .Where(x => x.Slug == slug && x.IsPublished == true)
                    .Single();

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
                    IsFeatured = post.IsFeatured,
                    // Author information would need to be fetched separately if needed
                    // For now, returning null as it was optional in the original model
                    Author = null
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching blog post",
                    error = ex.Message
                });
            }
        }

        // GET: api/blog/{slug}/related
        [HttpGet("{slug}/related")]
        public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetRelatedPosts(string slug)
        {
            try
            {
                var currentPostResponse = await _supabase
                    .From<BlogPostSupabase>()
                    .Where(x => x.Slug == slug)
                    .Get();

                var currentPost = currentPostResponse.Models.FirstOrDefault();
                if (currentPost == null)
                    return NotFound(new { message = "Blog post not found" });

                var relatedResponse = await _supabase
                    .From<BlogPostSupabase>()
                    .Where(x => x.IsPublished == true && x.Slug != slug && x.Category == currentPost.Category)
                    .Order(x => x.PublishedAt, Constants.Ordering.Descending)
                    .Limit(3)
                    .Get();

                var related = relatedResponse.Models.Select(p => new BlogPostDto
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
                }).ToList();

                return Ok(related);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching related blog posts",
                    error = ex.Message
                });
            }
        }

        // POST: api/blog (Admin only)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BlogPostSupabase>> CreatePost([FromBody] CreateBlogPostRequest request)
        {
            try
            {
                // Check if slug exists
                var existingResponse = await _supabase
                    .From<BlogPostSupabase>()
                    .Where(x => x.Slug == request.Slug)
                    .Get();

                if (existingResponse.Models.Any())
                    return BadRequest(new { message = "A post with this slug already exists" });

                var post = new BlogPostSupabase
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
                    AuthorId = request.AuthorId,
                    CreatedAt = DateTime.UtcNow
                };

                var response = await _supabase
                    .From<BlogPostSupabase>()
                    .Insert(post);

                return CreatedAtAction(nameof(GetPost), new { slug = post.Slug }, response.Models.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error creating blog post",
                    error = ex.Message
                });
            }
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


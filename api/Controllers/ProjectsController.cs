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
    public class ProjectsController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public ProjectsController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                var response = await _supabase
                    .From<ProjectSupabase>()
                    .Order(x => x.IsFeatured, Constants.Ordering.Descending)
                    .Order(x => x.DisplayOrder, Constants.Ordering.Ascending)
                    .Order(x => x.CreatedAt, Constants.Ordering.Descending)
                    .Get();

                var projects = response.Models.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Year = p.Year,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    IsFeatured = p.IsFeatured
                }).ToList();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching projects",
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        // GET: api/projects/featured
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetFeaturedProjects()
        {
            try
            {
                var response = await _supabase
                    .From<ProjectSupabase>()
                    // Supabase Where requires explicit comparisons; bare property throws parse error
                    .Where(x => x.IsFeatured == true)
                    .Order(x => x.DisplayOrder, Constants.Ordering.Ascending)
                    .Limit(4)
                    .Get();

                var projects = response.Models.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Year = p.Year,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    IsFeatured = p.IsFeatured
                }).ToList();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching featured projects",
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        // GET: api/projects/{slug}
        [HttpGet("{slug}")]
        public async Task<ActionResult<ProjectDetailDto>> GetProject(string slug)
        {
            try
            {
                var project = await _supabase
                    .From<ProjectSupabase>()
                    .Where(x => x.Slug == slug)
                    .Single();

                if (project == null)
                    return NotFound(new { message = "Project not found" });

                var dto = new ProjectDetailDto
                {
                    Id = project.Id,
                    Slug = project.Slug,
                    Title = project.Title,
                    Category = project.Category,
                    Year = project.Year,
                    Role = project.Role,
                    Description = project.Description,
                    Overview = project.Overview,
                    ImageUrl = project.ImageUrl,
                    LiveUrl = project.LiveUrl,
                    GithubUrl = project.GithubUrl,
                    TechStack = JsonSerializer.Deserialize<List<string>>(project.TechStack) ?? new List<string>(),
                    Features = JsonSerializer.Deserialize<List<FeatureDto>>(project.Features) ?? new List<FeatureDto>(),
                    GalleryImages = string.IsNullOrEmpty(project.GalleryImages) 
                        ? new List<string>() 
                        : JsonSerializer.Deserialize<List<string>>(project.GalleryImages) ?? new List<string>()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error fetching project",
                    error = ex.Message
                });
            }
        }

        // POST: api/projects (Admin only)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectSupabase>> CreateProject([FromBody] CreateProjectRequest request)
        {
            try
            {
                // Check if slug exists
                var existingResponse = await _supabase
                    .From<ProjectSupabase>()
                    .Where(x => x.Slug == request.Slug)
                    .Get();

                if (existingResponse.Models.Any())
                    return BadRequest(new { message = "A project with this slug already exists" });

                var project = new ProjectSupabase
                {
                    Slug = request.Slug,
                    Title = request.Title,
                    Category = request.Category,
                    Year = request.Year,
                    Role = request.Role,
                    Description = request.Description,
                    Overview = request.Overview,
                    ImageUrl = request.ImageUrl,
                    LiveUrl = request.LiveUrl,
                    GithubUrl = request.GithubUrl,
                    TechStack = JsonSerializer.Serialize(request.TechStack ?? new List<string>()),
                    Features = JsonSerializer.Serialize(request.Features ?? new List<FeatureDto>()),
                    GalleryImages = JsonSerializer.Serialize(request.GalleryImages ?? new List<string>()),
                    IsFeatured = request.IsFeatured,
                    DisplayOrder = request.DisplayOrder,
                    CreatedAt = DateTime.UtcNow
                };

                var response = await _supabase
                    .From<ProjectSupabase>()
                    .Insert(project);

                return CreatedAtAction(nameof(GetProject), new { slug = project.Slug }, response.Models.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Error creating project",
                    error = ex.Message
                });
            }
        }
    }

    // DTOs
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
    }

    public class ProjectDetailDto : ProjectDto
    {
        public string Role { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public string? LiveUrl { get; set; }
        public string? GithubUrl { get; set; }
        public List<string> TechStack { get; set; } = new();
        public List<FeatureDto> Features { get; set; } = new();
        public List<string> GalleryImages { get; set; } = new();
    }

    public class FeatureDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateProjectRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public string? ImageUrl { get; set; }
        public string? LiveUrl { get; set; }
        public string? GithubUrl { get; set; }
        public List<string>? TechStack { get; set; }
        public List<FeatureDto>? Features { get; set; }
        public List<string>? GalleryImages { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }
}


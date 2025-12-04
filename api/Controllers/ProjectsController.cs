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
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            var projects = await _context.Projects
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.CreatedAt)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Year = p.Year,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    IsFeatured = p.IsFeatured
                })
                .ToListAsync();

            return Ok(projects);
        }

        // GET: api/projects/featured
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetFeaturedProjects()
        {
            var projects = await _context.Projects
                .Where(p => p.IsFeatured)
                .OrderBy(p => p.DisplayOrder)
                .Take(4)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Title,
                    Category = p.Category,
                    Year = p.Year,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    IsFeatured = p.IsFeatured
                })
                .ToListAsync();

            return Ok(projects);
        }

        // GET: api/projects/{slug}
        [HttpGet("{slug}")]
        public async Task<ActionResult<ProjectDetailDto>> GetProject(string slug)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Slug == slug);

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

        // POST: api/projects (Admin only)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest request)
        {
            if (await _context.Projects.AnyAsync(p => p.Slug == request.Slug))
                return BadRequest(new { message = "A project with this slug already exists" });

            var project = new Project
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
                DisplayOrder = request.DisplayOrder
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { slug = project.Slug }, project);
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


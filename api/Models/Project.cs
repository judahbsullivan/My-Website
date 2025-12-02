using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(4)]
        public string Year { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public string? Overview { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(500)]
        public string? LiveUrl { get; set; }

        [MaxLength(500)]
        public string? GithubUrl { get; set; }

        // Stored as JSON array
        public string TechStack { get; set; } = "[]";

        // Stored as JSON array of {title, description}
        public string Features { get; set; } = "[]";

        // Stored as JSON array of image URLs
        public string? GalleryImages { get; set; }

        public bool IsFeatured { get; set; } = false;

        public int DisplayOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}


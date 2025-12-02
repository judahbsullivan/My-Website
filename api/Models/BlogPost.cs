using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Excerpt { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        // Stored as JSON array of strings
        public string Tags { get; set; } = "[]";

        public int ReadTimeMinutes { get; set; } = 5;

        public bool IsFeatured { get; set; } = false;

        public bool IsPublished { get; set; } = false;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign key to author
        public string? AuthorId { get; set; }
        public AppUser? Author { get; set; }
    }
}


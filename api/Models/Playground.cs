using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Playground
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        // Main code content (Vue component, HTML, CSS, JS, etc.)
        public string Code { get; set; } = string.Empty;

        // Code language/type: vue, html, css, js, animation, etc.
        [MaxLength(50)]
        public string CodeType { get; set; } = "vue";

        // Additional CSS if needed
        public string? Css { get; set; }

        // Additional JavaScript if needed
        public string? JavaScript { get; set; }

        // Dependencies (stored as JSON array)
        public string Dependencies { get; set; } = "[]";

        // Tags (stored as JSON array)
        public string Tags { get; set; } = "[]";

        // Preview image URL
        [MaxLength(500)]
        public string? PreviewImageUrl { get; set; }

        // Whether this is a Vue component that can be rendered
        public bool IsVueComponent { get; set; } = false;

        // Whether this is interactive (has user interactions)
        public bool IsInteractive { get; set; } = false;

        // View count
        public int ViewCount { get; set; } = 0;

        // Like count
        public int LikeCount { get; set; } = 0;

        public bool IsFeatured { get; set; } = false;

        public bool IsPublished { get; set; } = false;

        // Subscription: whether this playground requires a subscription to view full content
        public bool RequiresSubscription { get; set; } = false;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign key to author
        public string? AuthorId { get; set; }
        public AppUser? Author { get; set; }
    }
}


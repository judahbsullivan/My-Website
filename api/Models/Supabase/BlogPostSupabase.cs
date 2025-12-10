using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MyPortfolio.Models.Supabase
{
    [Table("BlogPosts")]
    public class BlogPostSupabase : BaseModel
    {
        [PrimaryKey("Id")]
        public int Id { get; set; }

        [Column("Slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Column("Category")]
        public string Category { get; set; } = string.Empty;

        [Column("Excerpt")]
        public string Excerpt { get; set; } = string.Empty;

        [Column("Content")]
        public string Content { get; set; } = string.Empty;

        [Column("Image")]
        public string? Image { get; set; }

        [Column("Tags")]
        public string Tags { get; set; } = "[]";

        [Column("ReadTimeMinutes")]
        public int ReadTimeMinutes { get; set; } = 5;

        [Column("IsFeatured")]
        public bool IsFeatured { get; set; } = false;

        [Column("IsPublished")]
        public bool IsPublished { get; set; } = false;

        [Column("PublishedAt")]
        public DateTime? PublishedAt { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Column("AuthorId")]
        public string? AuthorId { get; set; }
    }
}

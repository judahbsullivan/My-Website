using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MyPortfolio.Models.Supabase
{
    [Table("Playgrounds")]
    public class PlaygroundSupabase : BaseModel
    {
        [PrimaryKey("Id")]
        public int Id { get; set; }

        [Column("Slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Column("Category")]
        public string Category { get; set; } = string.Empty;

        [Column("Code")]
        public string Code { get; set; } = string.Empty;

        [Column("CodeType")]
        public string CodeType { get; set; } = "vue";

        [Column("Css")]
        public string? Css { get; set; }

        [Column("JavaScript")]
        public string? JavaScript { get; set; }

        [Column("Dependencies")]
        public string Dependencies { get; set; } = "[]";

        [Column("Tags")]
        public string Tags { get; set; } = "[]";

        [Column("PreviewImageUrl")]
        public string? PreviewImageUrl { get; set; }

        [Column("IsVueComponent")]
        public bool IsVueComponent { get; set; } = false;

        [Column("IsInteractive")]
        public bool IsInteractive { get; set; } = false;

        [Column("ViewCount")]
        public int ViewCount { get; set; } = 0;

        [Column("LikeCount")]
        public int LikeCount { get; set; } = 0;

        [Column("IsFeatured")]
        public bool IsFeatured { get; set; } = false;

        [Column("IsPublished")]
        public bool IsPublished { get; set; } = false;

        [Column("RequiresSubscription")]
        public bool RequiresSubscription { get; set; } = false;

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

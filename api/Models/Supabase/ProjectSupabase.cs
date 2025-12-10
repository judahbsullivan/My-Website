using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MyPortfolio.Models.Supabase
{
    [Table("Projects")]
    public class ProjectSupabase : BaseModel
    {
        [PrimaryKey("Id")]
        public int Id { get; set; }

        [Column("Slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Column("Category")]
        public string Category { get; set; } = string.Empty;

        [Column("Year")]
        public string Year { get; set; } = string.Empty;

        [Column("Role")]
        public string Role { get; set; } = string.Empty;

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Column("Overview")]
        public string? Overview { get; set; }

        [Column("ImageUrl")]
        public string? ImageUrl { get; set; }

        [Column("LiveUrl")]
        public string? LiveUrl { get; set; }

        [Column("GithubUrl")]
        public string? GithubUrl { get; set; }

        [Column("TechStack")]
        public string TechStack { get; set; } = "[]";

        [Column("Features")]
        public string Features { get; set; } = "[]";

        [Column("GalleryImages")]
        public string? GalleryImages { get; set; }

        [Column("IsFeatured")]
        public bool IsFeatured { get; set; } = false;

        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; } = 0;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}

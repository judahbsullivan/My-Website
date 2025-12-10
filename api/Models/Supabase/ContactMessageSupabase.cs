using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MyPortfolio.Models.Supabase
{
    [Table("ContactMessages")]
    public class ContactMessageSupabase : BaseModel
    {
        [PrimaryKey("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("Subject")]
        public string Subject { get; set; } = string.Empty;

        [Column("Message")]
        public string Message { get; set; } = string.Empty;

        [Column("IsRead")]
        public bool IsRead { get; set; } = false;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

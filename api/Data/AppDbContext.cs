using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Models;

namespace MyPortfolio.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Playground> Playgrounds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Project configuration
            builder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.TechStack).HasDefaultValue("[]");
                entity.Property(e => e.Features).HasDefaultValue("[]");
            });

            // BlogPost configuration
            builder.Entity<BlogPost>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Tags).HasDefaultValue("[]");
                entity.HasOne(e => e.Author)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ContactMessage configuration
            builder.Entity<ContactMessage>(entity =>
            {
                entity.HasIndex(e => e.CreatedAt);
            });

            // Playground configuration
            builder.Entity<Playground>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Dependencies).HasDefaultValue("[]");
                entity.Property(e => e.Tags).HasDefaultValue("[]");
                entity.HasOne(e => e.Author)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}

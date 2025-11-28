using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Models;

namespace MyPortfolio.Data
{
    // AppUser will be your Identity user
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Add your tables here later
        // public DbSet<Project> Projects { get; set; }
        // public DbSet<Blog> Blogs { get; set; }
    }
}

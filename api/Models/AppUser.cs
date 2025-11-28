using Microsoft.AspNetCore.Identity;

namespace MyPortfolio.Models
{
    // Custom user type that extends IdentityUser
    public class AppUser : IdentityUser
    {
        // Make it non-nullable with default empty string to avoid nullable warnings
        public string DisplayName { get; set; } = string.Empty;
    }
}

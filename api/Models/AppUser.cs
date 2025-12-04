using Microsoft.AspNetCore.Identity;

namespace MyPortfolio.Models
{
    // Custom user type that extends IdentityUser
    public class AppUser : IdentityUser
    {
        // Make it non-nullable with default empty string to avoid nullable warnings
        public string DisplayName { get; set; } = string.Empty;

        // Subscription fields
        public bool HasActiveSubscription { get; set; } = false;
        public DateTime? SubscriptionExpiresAt { get; set; }
    }
}

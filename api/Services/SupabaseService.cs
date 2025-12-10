using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Models;

namespace MyPortfolio.Api.Services
{
    /// <summary>
    /// Supabase service wrapper following official Supabase C# documentation.
    /// Reference: https://supabase.com/docs/reference/csharp/installing
    /// 
    /// This is a thin wrapper around Supabase.Client that provides
    /// dependency injection and follows the official initialization pattern.
    /// </summary>
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(Supabase.Client client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Get the underlying Supabase client instance.
        /// Use this to access all Supabase features directly.
        /// Example: service.Client.From&lt;ProjectSupabase&gt;().Get()
        /// </summary>
        public Supabase.Client Client => _client;
    }
}


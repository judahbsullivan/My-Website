using Supabase;
using Supabase.Gotrue;
using SupabaseClient = Supabase.Client;

namespace MyPortfolio.Api.Services;

/// <summary>
/// Production-ready Supabase auth service using official Supabase C# APIs.
/// Reference: https://supabase.com/docs/reference/csharp/auth-signup
/// </summary>
public class SupabaseAuthService
{
    private readonly SupabaseClient _client;

    public SupabaseAuthService(SupabaseClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Sign up a user with email/password.
    /// Uses the official SignUp method.
    /// </summary>
    public async Task<User?> SignUpAsync(string email, string password, SignUpOptions? options = null)
    {
        var response = await _client.Auth.SignUp(email, password, options ?? new SignUpOptions());
        return response?.User;
    }

    /// <summary>
    /// Sign in a user with email/password.
    /// Uses the official SignIn (password) method.
    /// </summary>
    public async Task<Session?> SignInAsync(string email, string password)
    {
        var response = await _client.Auth.SignIn(email, password);
        return response;
    }

    /// <summary>
    /// Sign out the current session (global by default).
    /// </summary>
    public async Task SignOutAsync(Supabase.Gotrue.Constants.SignOutScope scope = Supabase.Gotrue.Constants.SignOutScope.Global)
    {
        await _client.Auth.SignOut(scope);
    }

    /// <summary>
    /// Retrieve the current session (after initialization or sign-in).
    /// </summary>
    public Task<Session?> GetCurrentSessionAsync()
    {
        return Task.FromResult(_client.Auth.CurrentSession);
    }

    /// <summary>
    /// Refresh the current session using the refresh token.
    /// </summary>
    public async Task<Session?> RefreshSessionAsync()
    {
        var current = _client.Auth.CurrentSession;
        if (current == null)
        {
            return null;
        }

        await _client.Auth.RefreshToken();
        return _client.Auth.CurrentSession;
    }
}


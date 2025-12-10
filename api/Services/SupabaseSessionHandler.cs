using System.Text.Json;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;

namespace MyPortfolio.Api.Services;

/// <summary>
/// Persists Supabase sessions to disk to support auth auto-refresh and restore.
/// Implements IGotrueSessionPersistence per Supabase C# docs.
/// </summary>
public class SupabaseSessionHandler : IGotrueSessionPersistence<Session>
{
    private readonly string _sessionFilePath;

    public SupabaseSessionHandler(string sessionFilePath)
    {
        _sessionFilePath = sessionFilePath;
    }

    public void DestroySession()
    {
        if (File.Exists(_sessionFilePath))
        {
            File.Delete(_sessionFilePath);
        }
    }

    public Session? LoadSession()
    {
        if (!File.Exists(_sessionFilePath))
        {
            return null;
        }

        var json = File.ReadAllText(_sessionFilePath);
        return JsonSerializer.Deserialize<Session>(json);
    }

    public void SaveSession(Session session)
    {
        var dir = Path.GetDirectoryName(_sessionFilePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(session);
        File.WriteAllText(_sessionFilePath, json);
    }
}


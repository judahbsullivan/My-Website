using System.Text;
using AspNetCoreRateLimit;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyPortfolio.Api.Services;
using MyPortfolio.Data;
using MyPortfolio.Models;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Load .env (Development only)
// -------------------------
if (builder.Environment.IsDevelopment())
{
    // Try api/.env first, then ../.env as fallback
    var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
    if (!File.Exists(envPath))
    {
        envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
    }
    
    if (File.Exists(envPath))
    {
        Env.Load(envPath);
    }
}


// -------------------------
// Database Connection Configuration
// -------------------------
// Support both DATABASE_URL (full connection string) and individual environment variables
// Supabase requires direct connection (not pooler) for migrations and EF Core
// See: https://supabase.com/docs/guides/database/connecting

string connectionString;

// Check for DATABASE_URL first (Supabase provides this in connection string format)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    // DATABASE_URL format: postgresql://user:password@host:port/database?sslmode=require
    // Convert to Npgsql format
    if (databaseUrl.StartsWith("postgresql://") || databaseUrl.StartsWith("postgres://"))
    {
        try
        {
            var dbUri = new Uri(databaseUrl);
            var userInfo = dbUri.UserInfo.Split(':');
            var username = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : "";
            var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
            var host = dbUri.Host;
            var port = dbUri.Port > 0 ? dbUri.Port.ToString() : "5432";
            var database = dbUri.AbsolutePath.TrimStart('/');
            
            // Parse query string for SSL mode
            var sslMode = "Require";
            if (!string.IsNullOrEmpty(dbUri.Query))
            {
                var queryParams = dbUri.Query.TrimStart('?').Split('&')
                    .Select(p => p.Split('='))
                    .Where(p => p.Length == 2)
                    .ToDictionary(p => Uri.UnescapeDataString(p[0]), p => Uri.UnescapeDataString(p[1]), StringComparer.OrdinalIgnoreCase);
                
                if (queryParams.TryGetValue("sslmode", out var sslModeValue))
                {
                    sslMode = sslModeValue;
                }
            }
            
            connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode={sslMode};Trust Server Certificate=true";
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to parse DATABASE_URL: {ex.Message}. Expected format: postgresql://user:password@host:port/database", ex);
        }
    }
    else
    {
        // Assume it's already in Npgsql format
        connectionString = databaseUrl;
    }
}
else
{
    // Fall back to individual environment variables
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new Exception(
        "Database connection missing. Set either DATABASE_URL or DB_HOST, DB_NAME, DB_USER, DB_PASSWORD. " +
        "For Supabase: Use the direct connection string from Settings → Database → Connection string (not pooler). " +
        "Format: postgresql://postgres:[PASSWORD]@db.[PROJECT-REF].supabase.co:5432/postgres");
    
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
    var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception("DB_NAME missing");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? throw new Exception("DB_USER missing");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new Exception("DB_PASSWORD missing");
    var dbSslMode = Environment.GetEnvironmentVariable("DB_SSLMODE") ?? "Require";
    var dbTrustCert = Environment.GetEnvironmentVariable("DB_TRUST_CERT") ?? "true";
    
    // Auto-convert pooler to direct connection for Supabase (migrations require direct connection)
    if (dbHost.Contains("pooler.supabase.com"))
    {
        // Try to extract project ref from SUPABASE_URL and construct direct connection
        var supabaseUrlEnv = Environment.GetEnvironmentVariable("SUPABASE_URL");
        if (!string.IsNullOrEmpty(supabaseUrlEnv) && Uri.TryCreate(supabaseUrlEnv, UriKind.Absolute, out var supabaseUri))
        {
            // Extract project ref from SUPABASE_URL (e.g., https://csfuzhtznhkdzytfotet.supabase.co)
            var hostParts = supabaseUri.Host.Split('.');
            if (hostParts.Length >= 2 && hostParts[0] != "db")
            {
                var projectRef = hostParts[0];
                dbHost = $"db.{projectRef}.supabase.co";
            }
            else
            {
                throw new Exception(
                    "Cannot convert pooler to direct connection. " +
                    "Please set DB_HOST to the direct connection host (db.[PROJECT-REF].supabase.co). " +
                    "Find it in Supabase Dashboard → Settings → Database → Connection string → Direct connection");
            }
        }
        else
        {
            throw new Exception(
                "Pooler connection detected but cannot auto-convert. " +
                "EF Core migrations require DIRECT connection. " +
                "Set DB_HOST to: db.[PROJECT-REF].supabase.co " +
                "(Find PROJECT-REF in your SUPABASE_URL or Supabase Dashboard)");
        }
    }
    
    connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode={dbSslMode};Trust Server Certificate={dbTrustCert}";
}

// -------------------------
// Add Services
// -------------------------

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (Allow Nuxt frontend during dev)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                policy.AllowAnyHeader()
                     .AllowAnyMethod()
                     .WithOrigins("http://localhost:3000");
            }
            else
            {
                // Configure production origins
                var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries) 
                    ?? new[] { "https://judahsullivan.dev" };
                
                policy.AllowAnyHeader()
                     .AllowAnyMethod()
                     .WithOrigins(allowedOrigins)
                     .AllowCredentials();
            }
        }
    );
});

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "POST:/api/contact",
            Period = "1m",
            Limit = 5
        },
        new RateLimitRule
        {
            Endpoint = "POST:/api/auth/register",
            Period = "1h",
            Limit = 3
        },
        new RateLimitRule
        {
            Endpoint = "POST:/api/auth/login",
            Period = "15m",
            Limit = 5
        },
        new RateLimitRule
        {
            Endpoint = "POST:/api/auth/forgot-password",
            Period = "1h",
            Limit = 3
        }
    };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

// EF Core / Postgres
// Configure with connection retry and better error handling
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        // Enable retry on failure for transient errors
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    });
});

// Identity with secure password reset token configuration
builder
    .Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        // Password reset token expiration: 1 hour (3600 seconds)
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
        
        // Account lockout configuration
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        
        // Password requirements
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure token provider options for password reset
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    // Password reset tokens expire after 1 hour
    options.TokenLifespan = TimeSpan.FromHours(1);
});

// JWT Token Service
builder.Services.AddSingleton<TokenService>();

// JWT Authentication
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT_KEY missing");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MyPortfolio";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "MyPortfolioUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Email Service
builder.Services.AddSingleton<EmailService>();

// Supabase Client
// Following official Supabase C# docs: https://supabase.com/docs/reference/csharp/initializing
// The docs use SUPABASE_URL and SUPABASE_KEY (anon key for client-side)
// For server-side APIs, SUPABASE_SERVICE_ROLE_KEY is recommended for elevated privileges
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
if (string.IsNullOrEmpty(supabaseUrl))
{
    throw new Exception("SUPABASE_URL missing. Make sure SUPABASE_URL=https://... is set in your .env file (in api/ directory or project root).");
}

// Validate URL format
if (!Uri.TryCreate(supabaseUrl, UriKind.Absolute, out var uri) || uri.Scheme != "https")
{
    throw new Exception($"SUPABASE_URL is invalid. Expected format: https://xxxxx.supabase.co. Got: {supabaseUrl}");
}

// Check for SUPABASE_KEY first (official docs pattern), then fall back to service role or anon key
var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");
if (string.IsNullOrEmpty(supabaseKey))
{
    // Fallback: try service role key (recommended for server-side) or anon key
    supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_SERVICE_ROLE_KEY") 
                  ?? Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
}

if (string.IsNullOrEmpty(supabaseKey))
{
    throw new Exception(
        "SUPABASE_KEY missing. " +
        "Set SUPABASE_KEY (anon key) or SUPABASE_SERVICE_ROLE_KEY (recommended for server-side) in your .env file. " +
        "See: https://supabase.com/docs/reference/csharp/initializing");
}

var sessionPath = Path.Combine(AppContext.BaseDirectory, ".supabase", "session.json");
var supabaseOptions = new SupabaseOptions
{
    AutoConnectRealtime = false, // disable realtime auto-connect for API
    AutoRefreshToken = true,     // enable token auto-refresh per docs
    SessionHandler = new SupabaseSessionHandler(sessionPath)
};

// Register Supabase client as singleton (will be initialized after app build)
// Following official Supabase C# pattern: https://supabase.com/docs/reference/csharp/installing
builder.Services.AddSingleton<Supabase.Client>(provider =>
{
    var client = new Supabase.Client(supabaseUrl, supabaseKey, supabaseOptions);
    // Don't initialize here - will be initialized asynchronously after app build
    return client;
});

// Register SupabaseService wrapper for cleaner dependency injection
builder.Services.AddSingleton<MyPortfolio.Api.Services.SupabaseService>();
// Register SupabaseAuthService for auth flows
builder.Services.AddSingleton<MyPortfolio.Api.Services.SupabaseAuthService>();

// Add Controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

// -------------------------
// Initialize Supabase Client (after app build, before middleware)
// Following official pattern: https://supabase.com/docs/reference/csharp/installing
// -------------------------
try
{
    var supabaseClient = app.Services.GetRequiredService<Supabase.Client>();
    await supabaseClient.InitializeAsync();
}
catch (Exception ex)
{
    throw new Exception($"Failed to initialize Supabase client: {ex.Message}", ex);
}

// -------------------------
// Middleware Pipeline
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Only force HTTPS in production (Fly.io handles this)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseIpRateLimiting();
app.UseCors("AllowFrontend");

// Enable static file serving for uploaded images
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// -------------------------
// Test Endpoint
// -------------------------
app.MapGet(
        "/weatherforecast",
        () =>
        {
            var summaries = new[]
            {
                "Freezing",
                "Bracing",
                "Chilly",
                "Cool",
                "Mild",
                "Warm",
                "Balmy",
                "Hot",
                "Sweltering",
                "Scorching",
            };

            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();

            return forecast;
        }
    )
    .WithName("GetWeatherForecast");

app.Run();

// -------------------------
// Record Type
// -------------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

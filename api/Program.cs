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

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Load .env (Development only)
// -------------------------
if (builder.Environment.IsDevelopment())
{
    Env.Load("../.env"); // adjust if your .env is in root outside api/
}

var dbHost =
    Environment.GetEnvironmentVariable("DB_HOST") ?? throw new Exception("DB_HOST missing");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName =
    Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception("DB_NAME missing");
var dbUser =
    Environment.GetEnvironmentVariable("DB_USER") ?? throw new Exception("DB_USER missing");
var dbPassword =
    Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new Exception("DB_PASSWORD missing");

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
var connectionString =
    $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode=Require;Trust Server Certificate=true";
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

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

// Add Controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

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

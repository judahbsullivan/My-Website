using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Api.Services;
using MyPortfolio.Data;
using MyPortfolio.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Load .env
// -------------------------
Env.Load("../.env"); // adjust if your .env is in root outside api/

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
        policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000")
    );
});

// EF Core / Postgres
var connectionString =
    $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode=Require;Trust Server Certificate=true";
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Identity
builder
    .Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT Token Service
builder.Services.AddSingleton<TokenService>();

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

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
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

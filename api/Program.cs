var builder = WebApplication.CreateBuilder(args);

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
    ); // Nuxt dev URL
});

// Add Controllers later if needed
// builder.Services.AddControllers();

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

// Enable CORS
app.UseCors("AllowFrontend");

// -------------------------
// Example Endpoint
// -------------------------

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

app.MapGet(
        "/weatherforecast",
        () =>
        {
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

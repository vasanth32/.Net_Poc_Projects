using Consul;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Set Kestrel to listen on port 5001
builder.WebHost.UseUrls("http://localhost:5001");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Toggleable health state
bool isHealthy = true;

// Consul client configuration
var consulClient = new ConsulClient(config =>
{
    // Consul agent address (default for local agent)
    config.Address = new Uri("http://localhost:8500");
});

string serviceId = $"productservice-{Guid.NewGuid()}";

// Register service with Consul on startup
app.Lifetime.ApplicationStarted.Register(async () =>
{
    var registration = new AgentServiceRegistration
    {
        ID = serviceId, // Unique ID for this instance
        Name = "ProductService", // Service name for discovery
        Address = "localhost", // Service address
        Port = 5001, // Service port
        Tags = new[] { "product", "api" },
        Check = new AgentServiceCheck
        {
            HTTP = "http://localhost:5001/healthcontroller/health",
            Interval = TimeSpan.FromSeconds(10),
            Timeout = TimeSpan.FromSeconds(2),
            DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
        }
    };
    // Register with Consul
    await consulClient.Agent.ServiceRegister(registration);
    Console.WriteLine($"Registered ProductService with Consul as {serviceId}");
});

// Deregister service from Consul on shutdown
app.Lifetime.ApplicationStopping.Register(async () =>
{
    await consulClient.Agent.ServiceDeregister(serviceId);
    Console.WriteLine($"Deregistered ProductService from Consul: {serviceId}");
});

// Health check endpoint (toggleable)
app.MapGet("/health", () =>
{
    if (isHealthy)
        return Results.Json(new { status = "Healthy" }, statusCode: 200);
    else
        return Results.Json(new { status = "Unhealthy" }, statusCode: 500);
});

// Endpoint to toggle health state
app.MapPost("/toggle-health", () =>
{
    isHealthy = !isHealthy;
    return Results.Json(new { status = isHealthy ? "Healthy" : "Unhealthy" });
});

// Always healthy endpoints
app.MapGet("/healthy1", () => Results.Json(new { status = "Healthy1" }, statusCode: 200));
app.MapGet("/healthy2", () => Results.Json(new { status = "Healthy2" }, statusCode: 200));

// Always unhealthy endpoints
app.MapGet("/unhealthy1", () => Results.Json(new { status = "Unhealthy1" }, statusCode: 500));
app.MapGet("/unhealthy2", () => Results.Json(new { status = "Unhealthy2" }, statusCode: 500));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// Minimal /product endpoint
app.MapGet("/product", () => "Hello from ProductService (registered with Consul)!");

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

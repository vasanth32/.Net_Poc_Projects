using Consul;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Set Kestrel to listen on port 5002
builder.WebHost.UseUrls("http://localhost:5002");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

// Consul client configuration
var consulClient = new ConsulClient(config =>
{
    // Consul agent address (default for local agent)
    config.Address = new Uri("http://localhost:8500");
});

// Register ConsumerService with Consul
string serviceId = $"consumerservice-{Guid.NewGuid()}";

// Toggleable health state
bool isHealthy = true;

// Register service with Consul on startup
app.Lifetime.ApplicationStarted.Register(async () =>
{
    var registration = new AgentServiceRegistration
    {
        ID = serviceId,
        Name = "ConsumerService",
        Address = "localhost",
        Port = 5002,
        Tags = new[] { "consumer", "api" },
        Check = new AgentServiceCheck
        {
            HTTP = "http://localhost:5002/health",
            Interval = TimeSpan.FromSeconds(10),
            Timeout = TimeSpan.FromSeconds(2),
            DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
        }
    };
    await consulClient.Agent.ServiceRegister(registration);
    Console.WriteLine($"Registered ConsumerService with Consul as {serviceId}");
});

// Deregister service from Consul on shutdown
app.Lifetime.ApplicationStopping.Register(async () =>
{
    await consulClient.Agent.ServiceDeregister(serviceId);
    Console.WriteLine($"Deregistered ConsumerService from Consul: {serviceId}");
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

// /consume endpoint: discovers ProductService via Consul and calls its /product endpoint
app.MapGet("/consume", async () =>
{
    // Query Consul for healthy ProductService instances
    var services = await consulClient.Health.Service("ProductService", tag: null, passingOnly: true);
    var instance = services.Response.FirstOrDefault();
    if (instance == null)
    {
        return Results.Problem("No healthy ProductService instance found in Consul.");
    }
    // Build the URL to call ProductService
    var address = instance.Service.Address;
    var port = instance.Service.Port;
    var url = $"http://{address}:{port}/product";

    // Call ProductService
    using var http = new HttpClient();
    try
    {
        var response = await http.GetStringAsync(url);
        return Results.Ok($"ConsumerService received: {response}");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to call ProductService: {ex.Message}");
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

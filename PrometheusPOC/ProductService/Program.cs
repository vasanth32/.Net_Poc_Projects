using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Add this *before* any app.UseRouting()
app.UseMetricServer(); // default: exposes at /metrics
app.UseHttpMetrics();  // auto track HTTP request metrics

app.MapControllers();

app.Run();

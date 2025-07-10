using Prometheus;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Expose /metrics endpoint for Prometheus
app.UseMetricServer();
// Collect default HTTP metrics
app.UseHttpMetrics();

// Custom counter for /hello requests
static readonly Counter HelloCounter = Metrics.CreateCounter("servicea_hello_requests_total", "Number of /hello requests to ServiceA");

app.MapGet("/hello", () =>
{
    HelloCounter.Inc();
    return "Hello from ServiceA!";
});

app.Run(); 
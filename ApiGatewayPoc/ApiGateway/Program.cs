using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// Add YARP reverse proxy services from configuration
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Middleware to add a custom header indicating which service handled the request
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        // Set X-Service-Origin based on the route
        if (context.Request.Path.StartsWithSegments("/product"))
            context.Response.Headers["X-Service-Origin"] = "ProductService";
        else if (context.Request.Path.StartsWithSegments("/user"))
            context.Response.Headers["X-Service-Origin"] = "UserService";
        return Task.CompletedTask;
    });
    await next();
});

// Map the reverse proxy to handle incoming requests
app.MapReverseProxy();

app.Run();

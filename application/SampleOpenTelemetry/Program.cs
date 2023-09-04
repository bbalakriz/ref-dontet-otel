using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using System.Diagnostics;

// This is required if the collector doesn't expose an https endpoint
// AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(options =>
    {
        options.AddAspNetCoreInstrumentation();
        options.AddHttpClientInstrumentation();
        options.AddOtlpExporter(option => option.Endpoint = new Uri("http://otel-collector.otel.svc.cluster.local:4317"));
    });
    
builder.Services.AddOpenTelemetry()
    .WithMetrics(options => 
    {
        options.AddHttpClientInstrumentation();
        options.AddAspNetCoreInstrumentation();
        options.AddProcessInstrumentation();
        options.AddRuntimeInstrumentation();
        options.AddOtlpExporter(option => option.Endpoint = new Uri("http://otel-collector.otel.svc.cluster.local:4317"));
    });

var appResourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("ref-dontet-otel", "1.0");

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(appResourceBuilder);
        options.AddOtlpExporter(option =>
        {
            option.Endpoint = new Uri("http://otel-collector.otel.svc.cluster.local:4317");
        });
    });
});

var app = builder.Build();
var logger = loggerFactory.CreateLogger<Program>();
var httpClient = new HttpClient();

logger.LogInformation("********************************************************");
logger.LogInformation("Starting the app ***************************************");
logger.LogInformation("********************************************************");


app.MapGet("/", async() =>
{
    var response1 = httpClient.GetAsync("https://example.com");
    var response2 = httpClient.GetAsync("https://google.com");

    logger.LogInformation("********************API INVOKED************************************");
    logger.LogInformation("API1 Response: {content}", await response1.Result.Content.ReadAsStringAsync());
    logger.LogInformation("API2 Response: {content}", await response2.Result.Content.ReadAsStringAsync());
    logger.LogInformation("********************API INVOKED************************************");

    return Results.Ok();
});

app.Run();

using Microsoft.AspNetCore.HttpLogging;
using System.Diagnostics.Metrics;
using System.Diagnostics;

// This is required if the collector doesn't expose an https endpoint
// AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
var httpClient = new HttpClient();

app.MapGet("/", async() =>
{
    var response1 = httpClient.GetAsync("https://example.com");
    var response2 = httpClient.GetAsync("https://google.com");

    await response1.Result.Content.ReadAsStringAsync();
    await response2.Result.Content.ReadAsStringAsync();

    return Results.Ok("Success");
});

app.Run();

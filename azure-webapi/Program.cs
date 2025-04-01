using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Console.WriteLine($"Using port: {port}");
builder.WebHost.UseUrls($"https://*:{port}");

// Get Key Vault URI from configuration
var keyVaultUri = builder.Configuration["AzureKeyVault:VaultUri"];

if (!string.IsNullOrEmpty(keyVaultUri))
{
    var environment = builder.Environment.EnvironmentName;
    if (environment == "Production")
    {
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
    }
    else
    {
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new InteractiveBrowserCredential());
    }
}

// Code to fetch appinsights connection string from keyVault.
var appInsightsConnectionString = builder.Configuration[builder.Configuration["AzureKeyVault:appInsights"]];

if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = appInsightsConnectionString;
    });

    builder.Services.AddLogging(log =>
    {
        log.AddApplicationInsights();
        log.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
    });
}
else
{
    Console.WriteLine("Application Insights connection string is not configured.");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
Console.WriteLine("Hello World! This is a test for Azure Web API.");
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();

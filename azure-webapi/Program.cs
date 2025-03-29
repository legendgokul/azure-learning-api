var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Console.WriteLine($"Using port: {port}");
builder.WebHost.UseUrls($"http://*:{port}");

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

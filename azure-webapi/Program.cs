var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
Console.WriteLine("Hello World! This is a test for Azure Web API.");
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();
Console.WriteLine("before port");
app.Urls.Add("http://*:8080");
Console.WriteLine("port selection issue");
app.MapControllers();

app.Run();

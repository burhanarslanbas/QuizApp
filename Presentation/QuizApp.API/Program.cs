using QuizApp.Application;
using QuizApp.Infrastructure;
using QuizApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

// DI IoC
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
     .WithOrigins("https://localhost:3000", "http://localhost:3000") // React uygulamasının çalıştığı adresler
     .AllowAnyHeader() // Herhangi bir header'a izin ver
     .AllowAnyMethod() // Herhangi bir HTTP metoduna izin ver
));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // CORS ayarlarını uygulamak için UseCors() metodunu çağırıyoruz

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
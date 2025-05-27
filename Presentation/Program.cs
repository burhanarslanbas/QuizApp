using QuizApp.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Exception Middleware'i kaydet
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Exception Middleware'i en üstte olmalı
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run(); 
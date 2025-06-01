using QuizApp.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarları
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "http://localhost:5173") // React ve Vite için varsayılan portlar
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Exception Middleware'i kaydet
builder.Services.AddTransient<ExceptionMiddleware>();

// HTTPS ayarları
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7001; // HTTPS port numarası
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS middleware'ini UseRouting'den önce ekle
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

// Exception Middleware'i en üstte olmalı
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run(); 
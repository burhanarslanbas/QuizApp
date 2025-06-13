using FluentValidation;
using Microsoft.OpenApi.Models;
using QuizApp.Application;
using QuizApp.Application.Validators.Role;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Middleware;
using QuizApp.Persistence;
using QuizApp.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDevServer",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:5173", "https://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

// Service Registrations
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Add RoleSeeder
//builder.Services.AddScoped<RoleSeeder>();

// Core Services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleRequestValidator>();

// Swagger/OpenAPI yapılandırması
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizApp API", Version = "v1" });

    // JWT Authentication için Swagger yapılandırması
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed Roles
//using (var scope = app.Services.CreateScope())
//{
//    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
//    await roleSeeder.SeedAsync();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware sıralaması önemli
app.UseHttpsRedirection();
app.UseCors("AllowViteDevServer");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>(); // Global hata yönetimi
app.MapControllers();

app.Run();
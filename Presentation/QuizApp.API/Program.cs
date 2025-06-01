using FluentValidation;
using Microsoft.OpenApi.Models;
using QuizApp.Application;
using QuizApp.Application.Options;
using QuizApp.Application.Validators.Role;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Middleware;
using QuizApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Service Registrations
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Configure TokenOptions
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Token"));

// Globalization configuration
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(System.Globalization.CultureInfo.InvariantCulture);
    options.SupportedCultures = new List<System.Globalization.CultureInfo> { System.Globalization.CultureInfo.InvariantCulture };
    options.SupportedUICultures = new List<System.Globalization.CultureInfo> { System.Globalization.CultureInfo.InvariantCulture };
});

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

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
     .WithOrigins("https://localhost:3000", "http://localhost:3000") // React uygulamasının çalıştığı adresler
     .AllowAnyHeader() // Herhangi bir header'a izin ver
     .AllowAnyMethod() // Herhangi bir HTTP metoduna izin ver
));

// Swagger yapılandırması
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizApp API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
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

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization();
app.UseCors(); // CORS ayarlarını uygulamak için UseCors() metodunu çağırıyoruz

app.UseHttpsRedirection();

// Add exception handling middleware (should be before authentication/authorization)
app.UseMiddleware<ExceptionMiddleware>();

// Authentication ve Authorization sıralaması
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
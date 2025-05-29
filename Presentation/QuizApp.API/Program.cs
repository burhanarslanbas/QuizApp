using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizApp.Application;
using QuizApp.Application.Validators.Auth;
using QuizApp.Infrastructure;
using QuizApp.Persistence;
using System.Text;

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

// Swagger yapılandırması
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizApp API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
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

// JWT Authentication yapılandırması
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
        };
    });

// Rol bazlı yetkilendirme politikaları
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => 
        policy.RequireRole("Admin"));
    
    options.AddPolicy("RequireTeacherRole", policy => 
        policy.RequireRole("Teacher"));
    
    options.AddPolicy("RequireStudentRole", policy => 
        policy.RequireRole("Student"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // CORS ayarlarını uygulamak için UseCors() metodunu çağırıyoruz

app.UseHttpsRedirection();

// Authentication ve Authorization sıralaması
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
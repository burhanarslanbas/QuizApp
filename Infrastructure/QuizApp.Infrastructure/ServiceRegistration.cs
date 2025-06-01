using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizApp.Application.Services;
using QuizApp.Application.Services.Token;
using QuizApp.Infrastructure.Managers;
using QuizApp.Infrastructure.Managers.Token;
using System.Text;

namespace QuizApp.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Authentication Configuration
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidAudience = configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"] ?? throw new InvalidOperationException("Security key is not configured"))),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Swagger Configuration
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizApp API", Version = "v1" });
            //     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //     {
            //         Description = "JWT Authorization header using the Bearer scheme",
            //         Name = "Authorization",
            //         In = ParameterLocation.Header,
            //         Type = SecuritySchemeType.ApiKey,
            //         Scheme = "Bearer"
            //     });
            //     c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //     {
            //         {
            //             new OpenApiSecurityScheme
            //             {
            //                 Reference = new OpenApiReference
            //                 {
            //                     Type = ReferenceType.SecurityScheme,
            //                     Id = "Bearer"
            //                 }
            //             },
            //             Array.Empty<string>()
            //         }
            //     });
            // });

            // CORS Configuration
            services.AddCors(options => options.AddDefaultPolicy(policy => policy
                .WithOrigins("https://localhost:3000", "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
            ));

            // Manager DI Registrations
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<ITokenService, TokenManager>();
            services.AddScoped<ITokenBlacklistService, TokenBlacklistManager>();
            services.AddScoped<IRoleService, RoleManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IOptionService, OptionManager>();
            services.AddScoped<IQuestionService, QuestionManager>();
            services.AddScoped<IQuestionRepoService, QuestionRepoManager>();
            services.AddScoped<IQuizService, QuizManager>();
            services.AddScoped<IQuizResultService, QuizResultManager>();
            services.AddScoped<IUserAnswerService, UserAnswerManager>();
        }
    }
} 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
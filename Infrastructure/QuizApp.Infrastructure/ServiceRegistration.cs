using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QuizApp.Application.Services;
using QuizApp.Application.Services.Token;
using QuizApp.Domain.Entities.Identity;
using QuizApp.Infrastructure.Managers;
using QuizApp.Infrastructure.Managers.Token;
using QuizApp.Persistence.Contexts;
using System.Globalization;
using System.Text;

namespace QuizApp.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Globalization configuration
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);
                options.SupportedCultures = new List<CultureInfo> { CultureInfo.InvariantCulture };
                options.SupportedUICultures = new List<CultureInfo> { CultureInfo.InvariantCulture };
            });

            // Identity Configuration
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<QuizAppDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<RefreshTokenProvider<AppUser>>("RefreshTokenProvider");

            // JWT Authentication Configuration
            services.Configure<Application.Options.TokenOptions>(configuration.GetSection("TokenOptions"));
            var tokenOptions = configuration.GetSection("TokenOptions").Get<Application.Options.TokenOptions>();

            if (tokenOptions == null)
            {
                throw new InvalidOperationException("TokenOptions configuration is missing. Please check your appsettings.Development.json file.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Register services
            services.AddScoped<ITokenService, TokenManager>();
            services.AddScoped<IAuthService, AuthManager>();

            // Manager DI Registrations
            services.AddScoped<IRoleService, RoleManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IOptionService, OptionManager>();
            services.AddScoped<IQuestionService, QuestionManager>();
            services.AddScoped<IQuizQuestionService, QuizQuestionManager>();
            services.AddScoped<IQuestionRepoService, QuestionRepoManager>();
            services.AddScoped<IQuizService, QuizManager>();
            services.AddScoped<IQuizResultService, QuizResultManager>();
            services.AddScoped<IUserAnswerService, UserAnswerManager>();

            // User Service
            services.AddScoped<IUserService, UserManager>();

            // Old DbContext Configuration
            /* DbContext
            services.AddDbContext<QuizAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            */
        }
    }
}
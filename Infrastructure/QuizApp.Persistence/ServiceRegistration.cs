using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities.Identity;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories;

namespace QuizApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext Configuration
services.AddDbContext<QuizAppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});

            // Identity Configuration
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<QuizAppDbContext>()
            .AddDefaultTokenProviders();

            // Repositories Configuration
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<IOptionReadRepository, OptionReadRepository>();
            services.AddScoped<IOptionWriteRepository, OptionWriteRepository>();

            services.AddScoped<IQuestionReadRepository, QuestionReadRepository>();
            services.AddScoped<IQuestionWriteRepository, QuestionWriteRepository>();

            services.AddScoped<IQuestionRepoReadRepository, QuestionRepoReadRepository>();
            services.AddScoped<IQuestionRepoWriteRepository, QuestionRepoWriteRepository>();

            services.AddScoped<IQuizReadRepository, QuizReadRepository>();
            services.AddScoped<IQuizWriteRepository, QuizWriteRepository>();

            services.AddScoped<IQuizResultReadRepository, QuizResultReadRepository>();
            services.AddScoped<IQuizResultWriteRepository, QuizResultWriteRepository>();

            services.AddScoped<IUserAnswerReadRepository, UserAnswerReadRepository>();
            services.AddScoped<IUserAnswerWriteRepository, UserAnswerWriteRepository>();
        }
    }
}

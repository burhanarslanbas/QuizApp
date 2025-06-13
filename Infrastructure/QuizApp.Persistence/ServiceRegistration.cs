using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.Application.Repositories.Category;
using QuizApp.Application.Repositories.Option;
using QuizApp.Application.Repositories.Question;
using QuizApp.Application.Repositories.QuestionRepo;
using QuizApp.Application.Repositories.Quiz;
using QuizApp.Application.Repositories.QuizQuestion;
using QuizApp.Application.Repositories.QuizResult;
using QuizApp.Application.Repositories.UserAnswer;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Interceptors;
using QuizApp.Persistence.Repositories.Category;
using QuizApp.Persistence.Repositories.Option;
using QuizApp.Persistence.Repositories.Question;
using QuizApp.Persistence.Repositories.QuestionRepo;
using QuizApp.Persistence.Repositories.Quiz;
using QuizApp.Persistence.Repositories.QuizQuestion;
using QuizApp.Persistence.Repositories.QuizResult;
using QuizApp.Persistence.Repositories.UserAnswer;

namespace QuizApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Interceptor'ı singleton olarak kaydet
            services.AddSingleton<BaseEntitySaveChangesInterceptor>();

            // DbContext Configuration
            services.AddDbContext<QuizAppDbContext>(options =>
            {
                var connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>(
                    configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development"
                        ? "LocalConnection"
                        : "AzureConnection");

                options.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure());
            });

            // Quiz Repositories
            services.AddScoped<IQuizReadRepository, QuizReadRepository>();
            services.AddScoped<IQuizWriteRepository, QuizWriteRepository>();

            // Question Repositories
            services.AddScoped<IQuestionReadRepository, QuestionReadRepository>();
            services.AddScoped<IQuestionWriteRepository, QuestionWriteRepository>();

            // Option Repositories
            services.AddScoped<IOptionReadRepository, OptionReadRepository>();
            services.AddScoped<IOptionWriteRepository, OptionWriteRepository>();

            // Category Repositories
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            // QuizQuestion Repositories
            services.AddScoped<IQuizQuestionReadRepository, QuizQuestionReadRepository>();
            services.AddScoped<IQuizQuestionWriteRepository, QuizQuestionWriteRepository>();

            // QuestionRepo Repositories
            services.AddScoped<IQuestionRepoReadRepository, QuestionRepoReadRepository>();
            services.AddScoped<IQuestionRepoWriteRepository, QuestionRepoWriteRepository>();

            // QuizResult Repositories
            services.AddScoped<IQuizResultReadRepository, QuizResultReadRepository>();
            services.AddScoped<IQuizResultWriteRepository, QuizResultWriteRepository>();

            // UserAnswer Repositories
            services.AddScoped<IUserAnswerReadRepository, UserAnswerReadRepository>();
            services.AddScoped<IUserAnswerWriteRepository, UserAnswerWriteRepository>();
        }
    }
}

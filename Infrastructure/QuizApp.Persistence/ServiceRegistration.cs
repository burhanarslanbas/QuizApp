using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.Application.Repositories;
using QuizApp.Domain.Entities.Identity;
using QuizApp.Persistence.Contexts;
using QuizApp.Persistence.Repositories;

namespace QuizApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            // DbContext Configuration
            services.AddDbContext<QuizAppDbContext>(options =>
            {
                options.UseSqlServer("Server=BURHAN;Database=QuizAppDB;Trusted_Connection=True;TrustServerCertificate=True;");
            });

            // Identity Configuration
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<QuizAppDbContext>();

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

using Microsoft.Extensions.DependencyInjection;
using QuizApp.Application.Services;
using QuizApp.Infrastructure.Managers;

namespace QuizApp.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<IQuizService, QuizManager>();
        services.AddScoped<IQuestionService, QuestionManager>();
        services.AddScoped<IQuizResultService, QuizResultManager>();
        services.AddScoped<IOptionService, OptionManager>();
        services.AddScoped<IUserAnswerService, UserAnswerManager>();
        services.AddScoped<IQuestionRepoService, QuestionRepoManager>();
        return services;
    }
}

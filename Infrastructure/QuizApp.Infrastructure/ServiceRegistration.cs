using Microsoft.Extensions.DependencyInjection;
using QuizApp.Application.Services;
using QuizApp.Application.Services.Token;
using QuizApp.Infrastructure.Handlers.Token;
using QuizApp.Infrastructure.Managers;

namespace QuizApp.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHandler, TokenHandler>();
        // Registering Managers as Scoped Services
        services.AddScoped<ICategoryService, CategoryManager>();
        services.AddScoped<IQuizService, QuizManager>();
        services.AddScoped<IQuestionService, QuestionManager>();
        services.AddScoped<IQuizResultService, QuizResultManager>();
        services.AddScoped<IOptionService, OptionManager>();
        services.AddScoped<IUserAnswerService, UserAnswerManager>();
        services.AddScoped<IQuestionRepoService, QuestionRepoManager>();
        // Auth servisini ekle
        services.AddScoped<IAuthService, AuthManager>();
        return services;
    }
}

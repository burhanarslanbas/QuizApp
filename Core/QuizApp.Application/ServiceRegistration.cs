using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QuizApp.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper Configuration
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // FluentValidation Configuration
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

using CodeBridgeTestTask.Application.Logging;
using CodeBridgeTestTask.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CodeBridgeTestTask.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddScoped<ILoggerService, LoggerService>();


        }
    }
}


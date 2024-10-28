
using CodeBridgeTestTask.Core.Interfaces;
using CodeBridgeTestTask.Core.Repositories;
using CodeBridgeTestTask.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CodeBridgeTestTask.Persistence.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDogRepository, DogRepository>();
            //services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();
            //services.AddInMemoryRateLimiting();
            //services.AddHttpContextAccessor();



        }
    }
}

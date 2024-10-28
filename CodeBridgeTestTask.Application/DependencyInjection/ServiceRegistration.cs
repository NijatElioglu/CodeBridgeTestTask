using CodeBridgeTestTask.Application.DTO.Dog;
using CodeBridgeTestTask.Application.Features.Dog.Queries.GetAll;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CodeBridgeTestTask.Application.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly() ));

            services.AddScoped<IRequestHandler<GetDogsQuery, List<DogDTO>>, GetDogsQueryHandler>();
        }
    }
}

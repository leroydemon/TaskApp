using Domain.Interfaces;
using Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceExtensions
{
    public static class ServiceExtentions
    {
        //Useful way how add million service 
        public static IServiceCollection AddScopedService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            var repositoryAssembly = typeof(IEntity).Assembly;
            var repositoryTypes = repositoryAssembly.GetTypes()
                .Where(t => typeof(IEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var repoType in repositoryTypes)
            {
                var interfaceTypes = repoType.GetInterfaces()
                .Where(i => i != typeof(IEntity) && typeof(IEntity).IsAssignableFrom(i))
                .ToList();

                if (interfaceTypes.Any())
                {
                    foreach (var interfaceType in interfaceTypes)
                    {
                        services.AddScoped(interfaceType, repoType);
                    }
                }
            }

            return services;
        }
    }
}

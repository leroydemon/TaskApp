using Domain.Data;
using FluentValidation.AspNetCore;
using Infrastructure.MappingProfilies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection ServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddLogging();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddAutoMapper(typeof(TaskToDoProfile));

            services.AddDbContext<TaskAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}

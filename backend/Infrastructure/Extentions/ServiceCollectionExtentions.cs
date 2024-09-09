using Domain.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.MappingProfilies;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection ServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                         {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                         },
                         new string[] { }
                    }
                });
            });
            
            services.AddLogging();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddAutoMapper(typeof(TaskToDoProfile));
            services.AddValidatorsFromAssembly(typeof(TaskToDoDtoValidator).Assembly);

            services.AddDbContext<TaskAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

    }
}

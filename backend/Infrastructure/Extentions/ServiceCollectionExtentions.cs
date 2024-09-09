﻿using Domain.Data;
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
        // Extension method to configure and add services to the IServiceCollection
        public static IServiceCollection ServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                // Define the security scheme for Bearer token authentication
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Add a security requirement for the API
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

            // Configure FluentValidation for automatic validation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(typeof(TaskToDoDtoValidator).Assembly);

            services.AddAutoMapper(typeof(TaskToDoProfile));

            // Configure Entity Framework Core to use SQL Server with the specified connection string
            services.AddDbContext<TaskAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}

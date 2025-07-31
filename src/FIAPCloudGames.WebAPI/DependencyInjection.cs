using FIAPCloudGames.Domain.Requests;
using FIAPCloudGames.WebAPI.Validators;
using FIAPCloudGames.WebAPI.Services;
using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FIAPCloudGames.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<CreateGameRequest>, CreateGameRequestValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        
        services.AddHostedService<DatabaseMigrationService>();
        
        services.AddSwaggerConfiguration();

        return services;
    }

    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swagger.IncludeXmlComments(xmlPath);

            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "FIAPCloudGames - Project",
                Description = "Tech Challenge (FIAP Cloud Games) - API Swagger",
                Contact = new OpenApiContact
                {
                    Name = "Suporte",
                    Email = "fiap.8nett.techchallenge@gmail.com",
                    Url = new Uri("https://www.fiap.com.br/")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://www.fiap.com.br/")
                }
            });

            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Input the JWT like: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
    }
}

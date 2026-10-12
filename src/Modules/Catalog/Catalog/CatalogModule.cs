using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add services to the container

        // Api Endpoint services

        // Application Use Case services

        // Data- Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(
                sp.GetServices<ISaveChangesInterceptor>()
            );

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Configure the HTTP requirest pipeline.

        // 1. Use Api Endpoint Services

        // 2. Use Application Use Case Services

        // 3. Use DAta- Infrastructure Services
        app.UseMigration<CatalogDbContext>();


        return app;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMed.Infrastructure.Postgres.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        
        var efSettings = configuration.GetSection("EfCore")
            .Get<EfCoreSettings>();
        
        services.AddDbContext<ApiContext>((sp, options) =>
        {
            
            options
                .EnableSensitiveDataLogging(efSettings.EnableSensitiveDataLogging)
                .EnableDetailedErrors(efSettings.EnableDetailedErrors)
                .UseNpgsql(connectionString, postgres =>
                {
                    postgres.CommandTimeout(efSettings.CommandTimeout);
                });
        });

        return services;
    }
}
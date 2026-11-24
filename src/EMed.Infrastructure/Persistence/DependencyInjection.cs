using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMed.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        var efSettings = configuration.GetSection("EfCore")
            .Get<EfCoreSettings>();
        
        services.AddDbContext<ApiContext>((sp, options) =>
        {
            
            options
                .EnableSensitiveDataLogging(efSettings.EnableSensitiveDataLogging)
                .EnableDetailedErrors(efSettings.EnableDetailedErrors)
                .UseSqlite(connectionString, sqlite =>
                {
                    sqlite.CommandTimeout(efSettings.CommandTimeout);
                });
        });

        return services;
    }
}
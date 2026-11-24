using EMed.Application.Interfaces.Caching;
using EMed.Infrastructure.Postgres.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis")
                                    ?? throw new InvalidOperationException("Redis connection string is missing.");

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddSingleton<RedisCacheService>();
        services.AddSingleton<ICacheService>(sp => sp.GetRequiredService<RedisCacheService>());
        services.AddSingleton<ICacheInvalidator>(sp => sp.GetRequiredService<RedisCacheService>());
        
        return services;
    }
}
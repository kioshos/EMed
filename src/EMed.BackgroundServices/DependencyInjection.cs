using Microsoft.Extensions.DependencyInjection;

namespace EMed.BackgroundServices;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundWorkers(this IServiceCollection services)
    {
        services.AddHostedService<AppointmentLoggingConsumer>();
        return services;
    }
}
using EMed.Infrastructure.Postgres.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;


namespace EMed.Tests.Integration;

public class IntegrationTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _sqlContainer;
    
    public IntegrationTestWebFactory()
    {
        _sqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("EMed")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => 
                    s.ServiceType == typeof(DbContextOptions<ApiContext>));
            if (descriptor != null)
            { 
                services.Remove(descriptor);
            }
            services.AddDbContext<ApiContext>(options =>
            {
                options.UseNpgsql(_sqlContainer.GetConnectionString());
            });
        });
        
    }
    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        var migrationSql = await File.ReadAllTextAsync("migration.sql");
        await _sqlContainer.ExecScriptAsync(migrationSql);
    }

    public new Task DisposeAsync()
    {
       return _sqlContainer.StopAsync();
    }
}
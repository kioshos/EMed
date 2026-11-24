using EMed.Infrastructure.Postgres.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EMed.Tests.Integration;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected IServiceProvider ServiceProvider => _scope.ServiceProvider;
    protected readonly ApiContext ApiContext;
    protected BaseIntegrationTest(IntegrationTestWebFactory factory)
    {
        _scope = factory.Services.CreateScope();
        ApiContext = _scope.ServiceProvider.GetRequiredService<ApiContext>();
    }
    public void Dispose()
    {
        _scope.Dispose();
    }
}
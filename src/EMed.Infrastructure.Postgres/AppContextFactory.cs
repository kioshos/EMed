using EMed.Infrastructure.Postgres.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EMed.Infrastructure.Postgres;

public class AppContextFactory : IDesignTimeDbContextFactory<ApiContext>
{
    public ApiContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
        optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=EMed;Username=postgres;Password=postgres;Maximum Pool Size=100;Minimum Pool Size = 10");

        return new ApiContext(optionsBuilder.Options);
    }
}
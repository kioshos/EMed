using EMed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EMed.Infrastructure;

public class AppContextFactory : IDesignTimeDbContextFactory<ApiContext>
{
    public ApiContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
        optionsBuilder.UseSqlite("Data Source=EMed.db");

        return new ApiContext(optionsBuilder.Options);
    }
}
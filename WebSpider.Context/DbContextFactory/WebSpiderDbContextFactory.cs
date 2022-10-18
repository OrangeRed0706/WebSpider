using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebSpider.Context.DbContextFactory
{
    public sealed class WebSpiderDbContextFactory : IDesignTimeDbContextFactory<SpiderDbContext>
    {
        public SpiderDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var connectionString = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build()
                .GetConnectionString("SpiderDbContext");

            var optionsBuilder = new DbContextOptionsBuilder<SpiderDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new SpiderDbContext(optionsBuilder.Options);
        }
    }
}

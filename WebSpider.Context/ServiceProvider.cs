using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSpiderDbContext.Interface;

namespace WebSpider.Context
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddSpiderDbContext(this IServiceCollection services, IConfiguration configuration, int poolSize = 128)
        {
            var connectionString = configuration.GetConnectionString("SpiderDbContext");

            services.AddDbContextPool<ISpiderDbContext, SpiderDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlite(
                    connectionString,
                    sqlLiteOptIons =>
                    {
                        sqlLiteOptIons.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    });
#if DEBUG
                //optionsBuilder.EnableDetailedErrors();
                //optionsBuilder.EnableSensitiveDataLogging();
#endif
            }, poolSize);

            return services;
        }

    }
}

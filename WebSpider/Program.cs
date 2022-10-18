using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebSpider.Accessor;
using WebSpider.Accessor.Interface;
using WebSpider.Context;
using WebSpider.Context.DbContextFactory;
using WebSpider.Interface;
using WebSpider.Job;
using WebSpider.Options;
using WebSpider.Services;
using WebSpider.Services.Interface;
using WebSpider.Utility;
using WebSpider.Utility.Interface;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 24 * 30 // 最多保留 30 天份的 Log 檔案
    )
);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHealthChecks();
services.AddSingleton<WebSpiderDbContextFactory>();
//Accessor
services.AddSingleton<ISqlLiteAccessor, SqlLiteAccessor>();
//services
services.AddSingleton<IBahamutSpiderServices, BahamutSpiderServices>();
//Utility
services.AddSingleton<ISendMessage, SendMessageServices>();
//Job
services.AddSingleton<IBahamutSpiderJob, BahamutSpiderJob>();

services.AddHangfire(hangFireConfig =>
{
    hangFireConfig.UseInMemoryStorage();
    RecurringJob.AddOrUpdate<IBahamutSpiderJob>("test", x => x.RunJob(), "0 */5 * ? * *");
});
services.AddHangfireServer();
services.AddSpiderDbContext(builder.Configuration);

services.Configure<DiscordWebHookOption>(configuration.GetSection("DiscordWebHook"));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet($"/selfhealthz", async context =>
    {
        await context.Response.WriteAsync("Web Spider Worker").ConfigureAwait(false);
    });
});

app.UseHttpsRedirection();
app.MapControllers();
app.UseHangfireDashboard();

using (var serviceScope = app.Services.CreateScope())
{
    var service = serviceScope.ServiceProvider;
    var factory = service.GetRequiredService<WebSpiderDbContextFactory>();
    var spiderDbContext = factory.CreateDbContext(args);
    var spiderDatabase = spiderDbContext.Database;
    try
    {
        spiderDatabase.Migrate();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}


app.Run();


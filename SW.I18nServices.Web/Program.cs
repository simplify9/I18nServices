using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SW.EfCoreExtensions;
using SW.I18nServices.Api;
using SW.Logger;

namespace SW.I18nServices.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).UseSwLogger().Build().MigrateDatabase<I18nServicesDbContext>().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

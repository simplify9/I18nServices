using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using SW.HttpExtensions;
using SW.I18nServices.Api;
using SW.Logger;
using SW.PrimitiveTypes;

namespace SW.I18nServices.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<I18nServicesDbContext>(c =>
            {
                c.EnableSensitiveDataLogging(true);
                c.UseMySql(Configuration.GetConnectionString(I18nServicesDbContext.ConnectionString), b =>
                {
                    b.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                    b.CommandTimeout(90);
                    b.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                });
            });

            services.AddScoped<RequestContext>();  

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpUserRequestContext();
            app.UseRequestContextLogEnricher();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

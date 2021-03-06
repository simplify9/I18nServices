using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using SW.CqApi;
using SW.HttpExtensions;
using SW.I18nService;
using SW.I18nService.Resources.Localities;
using SW.I18nServices.Api;
using SW.I18nServices.Populator;
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
            AddCookie(configureOptions =>
            {
                configureOptions.LoginPath = "/login";
            }).
            AddJwtBearer();

            services.AddCqApi(typeof(GetCountryLocalities).Assembly);

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
            services.AddScoped<RetrievalService>();
            services.AddScoped<PlaceDb>();

            var i18nOptions = new I18nOptions();
            if (Configuration != null) Configuration.GetSection(I18nOptions.ConfigurationSection).Bind(i18nOptions);
            services.AddSingleton(i18nOptions);
            //services.AddHostedService<FileWatcher>(f => {
            //    return new FileWatcher(f.GetService<ILogger<FileWatcher>>(), Configuration);
            //});
            services.AddHostedService<FileWatcher>();
            services.AddMemoryCache();
            services.AddTransient<I18nServiceService>();
            services.AddSingleton<CountriesService>();
            services.AddSingleton<CurrenciesService>();
            services.AddSingleton<PhoneNumberingPlansService>();
            services.AddHttpClient<ExternalCurrencyRatesService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
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

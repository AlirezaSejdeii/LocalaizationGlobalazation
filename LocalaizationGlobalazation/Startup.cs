using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LocalaizationGlobalazation
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
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(opt=> {
                var supportedCulters = new List<CultureInfo>()
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar"),
                    new CultureInfo("fa")
                };
                opt.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                opt.SupportedCultures = supportedCulters;
                opt.SupportedUICultures = supportedCulters;
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

            //var supportedCulture = new[] {"en","fa","ar"};
            //var localazationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCulture[0]).AddSupportedCultures(supportedCulture).AddSupportedUICultures(supportedCulture);
            //app.UseRequestLocalization(localazationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

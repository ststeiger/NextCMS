
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace NextCMS
{

    public class Startup
        : IStartup
    {
        public IConfiguration Configuration { get; }
        private IApplicationBuilder m_Application;


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        void IStartup.Configure(IApplicationBuilder app)
        {
            this.m_Application = app;

            Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = app.ApplicationServices.
                GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();

            Microsoft.AspNetCore.Hosting.IHostingEnvironment env = app.ApplicationServices.
                GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();

            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext = app.ApplicationServices.
                GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();

            this.Configure(app, env);
        }

        IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            // https://stackoverflow.com/questions/48373229/400-bad-request-when-post-ing-to-razor-page
            // https://www.talkingdotnet.com/disable-antiforgery-token-validation-globally-asp-net-core-razor-pages/
            services.AddMvc()
            /*
            services.AddMvc().AddRazorPagesOptions(o =>
            {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            })
            */
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            ;

            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                // options.FileProvider = new BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem.AzureFileProvider(this.Configuration);

                options.FileProviders.Add(
                    // new BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem.AzureFileProvider(this.Configuration)
                    new DbFileSystem.DbFileProvider(this.Configuration)
                );

            });
            

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<Services.IViewRenderService, Services.ViewRenderService>();

            return services.BuildServiceProvider();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }


    }


}

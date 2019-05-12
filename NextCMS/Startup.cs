
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
    {


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                // options.FileProvider = new BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem.AzureFileProvider(this.Configuration);

                options.FileProviders.Add(
                    // new BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem.AzureFileProvider(this.Configuration)
                    new DbFileSystem.DbFileProvider(this.Configuration)
                );

            });


            services.AddScoped<Services.IViewRenderService, Services.ViewRenderService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

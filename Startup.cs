using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TCM.Models;
using AutoMapper;
using TCM.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TCM
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddSingleton(Configuration);
            services.AddDbContext<TCMContext>();
            //adding transient object - app is going to create it every time we need it; transient is used if service doesn't have own state; we will get a copy each time.
            services.AddTransient<TCMContextSeedData>();
            //here we will use AddScoped to create it once per request cycle - as arguments, we supply interface and instance of a class we want to use
            services.AddScoped<ITCMRepository, TCMRepository>();
            //for the test project we would do something like: because we would have a different implementation of that repository
            //services.AddScoped<ITCMRepository, MockTCMRepository>();
            services.AddMvc();
            services.AddIdentity<TCMUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath="/Auth/Login";
            })
            .AddEntityFrameworkStores<TCMContext>();
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // we also need to inject our seeder directly into the Configure method
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            TCMContextSeedData seeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIdentity();
            app.UseApplicationInsightsRequestTelemetry();

            Mapper.Initialize(config =>
            {
                config.CreateMap<TestCaseViewModel, TestCase>().ReverseMap();
                config.CreateMap<StepViewModel, Step>().ReverseMap();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/App/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=App}/{action=Index}/{id?}");
            });

            //and now we need to call the seeder
            seeder.EnsureSeedData().Wait();
        }
    }
}

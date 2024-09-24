using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FS.Farm.FSFarmAPI
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
            services.AddSignalR();
            services.AddCors(options =>
            {
                //options.AddPolicy("AnyOrigin", builder =>
                //{
                //    // builder.WithOrigins("http://localhost:3000,https://localhost:3000".Split(','));
                //    //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                //    builder.WithOrigins("https://agreeable-hill-0bf1fbf0f.5.azurestaticapps.net", "http://localhost:3000", "https://localhost:3000")
                //       .AllowCredentials()
                //       .AllowAnyHeader()
                //       .AllowAnyMethod()
                //       .WithExposedHeaders("Content-Disposition");

                //});

                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                            .WithExposedHeaders("Content-Disposition");
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                //app.UseCors(
                //    options => options.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
                //);
            }

            app.UseRouting();

            app.UseCors("AnyOrigin");
            app.UseAuthorization(); 


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<FS.Farm.FSFarmAPI.SignalR.AnalyticsHub>("/analytics-hub");
            });
        }
    }
}

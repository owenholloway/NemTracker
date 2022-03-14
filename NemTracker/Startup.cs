using System;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NemTracker.Services;

namespace NemTracker
{
    public class Startup
    {

        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            Console.WriteLine("ConfigureServices Start");
            
            //Config.Version = _configuration.GetSection("Misc").GetSection("Version").Value;
            
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddMvcCore();

            Console.WriteLine("ConfigureServices Completed");

        }
        
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Configure Start");
            
            if (env.IsDevelopment())
            {
                //TODO
            }

            
            /*
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoEuS V1");
            });*/

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            Console.WriteLine("Configure End");
            
        }
        
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            Console.WriteLine("ConfigureContainer Start");

            
            builder.RegisterModule(new Persistence.Features.Persistence());
            builder.RegisterAssemblyModules(AppScanner.GetAssemblies());
            
            Console.WriteLine("ConfigureContainer End");
        }
        
    }
}
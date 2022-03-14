using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NemTracker.Features;
using NemTracker.Persistence.Features;
using NemTracker.Services;

namespace NemTracker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            try
            {

                var builder = WebApplication.CreateBuilder(args);

                builder.Host
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory());

                builder.Host.ConfigureHostOptions(o => 
                    o.ShutdownTimeout = TimeSpan.FromSeconds(30));
                
                builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => 
                    containerBuilder.RegisterModule(new Persistence.Features.Persistence()));
                
                builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => 
                    containerBuilder.RegisterAssemblyModules(AppScanner.GetAssemblies()));

                builder.Services.AddDbContext<NEMDBContext>(options => 
                    options.UseNpgsql("Host=172.16.40.100;Database=nemtracker.test;" +
                                      "Username=nemtracker.test;Password=DsF82VQZ8ZqizkDhdxTHjE2mqfBeDdzL"));
                
                var application = builder.Build();
                
                application.Run();
                

                //CreateWebHostBuilder(args).Run();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            
            
            //var nemProcessor = new NemRegistrationsProcessor();
            //var stations = nemProcessor.GetStations();
            /*
            foreach (var station in stations)
            {
                Console.Write("Station: ");
                Console.Write(station.StationName);
                Console.Write(", ");
                
                Console.Write("Units Min: ");
                Console.Write(station.PhysicalUnitMin);
                Console.Write(", ");
                
                Console.Write("Units Max: ");
                Console.Write(station.PhysicalUnitMax);
                Console.WriteLine();
            }*/

        }
        /*
        private static IWebHost CreateWebHostBuilder(string[] args)
        {
            
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var webHost = WebHost
                .CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();

            return webHost;
        }*/
        
    }
}
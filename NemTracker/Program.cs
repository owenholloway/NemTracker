using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NemTracker.Features;
using NemTracker.Services;

namespace NemTracker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            try
            {
                CreateWebHostBuilder(args, configuration).Run();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }

        }
        
        private static IWebHost CreateWebHostBuilder(string[] args, IConfiguration configuration)
        {
            
            var webHost = WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(service => service.AddAutofac())
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();

            return webHost;
        }
        
    }
}
using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NemTracker.Services;

namespace NemTracker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {

                CreateWebHostBuilder(args).Run();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }

        }
        
        private static IWebHost CreateWebHostBuilder(string[] args)
        {
            
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

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
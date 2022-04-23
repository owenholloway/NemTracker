using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Persistence.Features
{
    public class Persistence : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ReadOnlyRepository>()
                .As<IReadOnlyRepository>().InstancePerLifetimeScope();
            
            builder.RegisterType<ReadWriteRepository>()
                .As<IReadWriteRepository>().InstancePerLifetimeScope();
            
            //Db Context Options
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder();
                var connstr = config.GetConnectionString("ApplicationDatabase");
                optionsBuilder.UseNpgsql(config.GetConnectionString("ApplicationDatabase"));
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
                return optionsBuilder.Options;
            }).As<DbContextOptions>().InstancePerDependency();
            
            builder.RegisterType<NEMDBContext>().As<DbContext>().InstancePerLifetimeScope();

        }
    }
}
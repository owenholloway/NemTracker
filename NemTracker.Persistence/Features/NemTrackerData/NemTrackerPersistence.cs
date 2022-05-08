using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Persistence.Features.NemTrackerData
{
    public class NemTrackerPersistence : Module
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
                var optionsBuilder = new DbContextOptionsBuilder();
                var connstr = Environment.GetEnvironmentVariable("APPLICATION_DATABASE");
                optionsBuilder.UseNpgsql(connstr);
                return optionsBuilder.Options;
            }).As<DbContextOptions>().InstancePerDependency();
            
            builder.RegisterType<NEMTrackerContext>().As<DbContext>().InstancePerLifetimeScope();

        }
    }
}
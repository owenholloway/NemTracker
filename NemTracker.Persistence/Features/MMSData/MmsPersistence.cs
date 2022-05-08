using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using NemTracker.Persistence.Features.NemTrackerData;
using NemTracker.Persistence.Interfaces;

namespace NemTracker.Persistence.Features.MMSData
{
    public class MmsPersistence : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<MmsReadOnlyRepository>()
                .As<IMmsReadOnlyRepository>().InstancePerLifetimeScope();

            //Db Context Options
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                var connstr = Environment.GetEnvironmentVariable("MMS_DATABASE");
                optionsBuilder.UseNpgsql(connstr);
                var dbContextOptions = optionsBuilder.Options;
                return new MmsDbContextContainer()
                {
                    ContextOptions = dbContextOptions
                };
            }).As<MmsDbContextContainer>().InstancePerDependency();
            
            builder.RegisterType<MmsDbContext>().InstancePerLifetimeScope();

        }
    }
}
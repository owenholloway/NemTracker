using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                return optionsBuilder.Options;
            }).As<DbContextOptions>().SingleInstance();
            
            builder.RegisterType<NEMDBContext>().As<DbContext>().InstancePerLifetimeScope();

        }
    }
}
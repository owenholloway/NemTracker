using Autofac;
using AutoMapper;
using NemTracker.Persistence.Features.MapperConfigurations;

namespace NemTracker.Persistence.Features
{
    public class AutoMapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<MapperConfiguration>(c 
                    => new MapperConfiguration(config =>
                    {
                        config.AddP5Reports();
                        
                    }))
                .InstancePerLifetimeScope();
        }

    }

}
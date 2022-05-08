using AutoMapper;
using NemTracker.Dtos.MmsData;
using NemTracker.Model.Model.MmsData.Dispatch;

namespace NemTracker.Persistence.Features.MapperConfigurations
{
    public static class DispatchMapping
    {
        public static void AddDispatch(this IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateProjection<DispatchLoad, DispatchLoadTinyDto>();
        }
    }
}
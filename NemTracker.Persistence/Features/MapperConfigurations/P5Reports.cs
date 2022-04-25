using AutoMapper;
using NemTracker.Dtos.Reports.Results;
using NemTracker.Model.Model.Reports;

namespace NemTracker.Persistence.Features.MapperConfigurations
{
    internal static class P5Reports
    {
        public static void AddP5Reports(this IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateProjection<RegionSolution, RegionSolutionRrpDto>();
        }
    }
}
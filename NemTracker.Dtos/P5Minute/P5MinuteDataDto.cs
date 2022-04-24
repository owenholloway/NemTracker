using System.Collections.Generic;

namespace NemTracker.Dtos.P5Minute
{
    public class P5MinuteDataDto
    {
        public List<ReportDto> IntervalDtos = new List<ReportDto>();
        public List<RegionSolutionDto> RegionSolutionDtos = new List<RegionSolutionDto>();
    }
}
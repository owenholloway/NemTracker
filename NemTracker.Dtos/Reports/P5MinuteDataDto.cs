using System.Collections.Generic;

namespace NemTracker.Dtos.Reports
{
    public class P5MinuteDataDto
    {
        public List<ReportDto> ReportDtos = new List<ReportDto>();
        public List<RegionSolutionDto> RegionSolutionDtos = new List<RegionSolutionDto>();
    }
}
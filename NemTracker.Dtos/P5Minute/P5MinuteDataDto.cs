using System.Collections.Generic;

namespace NemTracker.Dtos.P5Minute
{
    public class P5MinuteDataDto
    {
        public List<IntervalDto> IntervalDtos = new List<IntervalDto>();
        public List<RegionSolutionDto> RegionSolutionDtos = new List<RegionSolutionDto>();
    }
}
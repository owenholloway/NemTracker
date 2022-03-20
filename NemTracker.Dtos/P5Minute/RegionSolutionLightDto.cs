using System;
using NemTracker.Dtos.Stations;

namespace NemTracker.Dtos.P5Minute
{
    public class RegionSolutionLightDto
    {
        public long Id { get; set; }
        public DateTime RunTime { get; set; }
        public DateTime Interval { get; set; }
        public RegionEnum Region { get; set; }
    }
}
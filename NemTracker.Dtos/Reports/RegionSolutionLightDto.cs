using System;
using NemTracker.Dtos.Aemo;

namespace NemTracker.Dtos.Reports
{
    public class RegionSolutionLightDto
    {
        public long Id { get; set; }
        public DateTime RunTime { get; set; }
        public DateTime Interval { get; set; }
        public RegionEnum Region { get; set; }
    }
}
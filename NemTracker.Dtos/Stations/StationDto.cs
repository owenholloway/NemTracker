using System;
using NemTracker.Dtos.Aemo;

namespace NemTracker.Dtos.Stations
{
    public class StationDto
    {
        // ReSharper disable InconsistentNaming
        public long Id { get; set; }
        public long ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string StationName { get; set; }
        public RegionEnum Region { get; set; }
        public TechnologyTypeEnum TechnologyType { get; set; }
        public TechnologyTypeDescriptorEnum TechnologyTypeDescriptor { get; set; }
        public int PhysicalUnitMin { get; set; }
        public int PhysicalUnitMax { get; set; }
        public double UnitSizeMW { get; set; } = 0;
        public string DUID { get; set; }
        public DispatchTypeEnum DispatchType { get; set; } = DispatchTypeEnum.Undefined;
    }
}
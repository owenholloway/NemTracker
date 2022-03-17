using System;

namespace NemTracker.Dtos.Stations
{
    public class StationDto
    {
        // ReSharper disable InconsistentNaming
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
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
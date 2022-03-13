using System;


namespace NemTracker.Dtos
{
    public class StationDto
    {
        // ReSharper disable InconsistentNaming
        public Guid ParticipantId { get; set; }
        public string StationName { get; set; }
        public int PhysicalUnitMin { get; set; }
        public int PhysicalUnitMax { get; set; }
        public double UnitSizeMW { get; set; } = 0;
        public string DUID { get; set; }
        public DispatchType DispatchType { get; set; } = DispatchType.Undefined;
    }
}
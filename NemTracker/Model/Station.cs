using System;
using NemTracker.Dtos;

namespace NemTracker.Model
{
    public class Station
    {
        
        // ReSharper disable InconsistentNaming
        public Guid ParticipantId { get; private set; }
        public string StationName { get; private set; }
        public RegionEnum Region { get; private set; }
        public TechnologyTypeEnum TechnologyType { get; private set; }
        public TechnologyTypeDescriptorEnum TechnologyTypeDescriptor { get; private set; }
        public int PhysicalUnitMin { get; private set; }
        public int PhysicalUnitMax { get; private set; }
        public double UnitSizeMW { get; private set; } = 0;
        public string DUID { get; private set; }
        public DispatchTypeEnum DispatchTypeEnum { get; private set; } = DispatchTypeEnum.Undefined;
        
        
        public static Station Create(StationDto dto)
        {
            var obj = new Station
            {
                ParticipantId = dto.ParticipantId,
                StationName = dto.StationName,
                Region = dto.Region,
                TechnologyType = dto.TechnologyType,
                TechnologyTypeDescriptor = dto.TechnologyTypeDescriptor,
                PhysicalUnitMin = dto.PhysicalUnitMin,
                PhysicalUnitMax = dto.PhysicalUnitMax,
                UnitSizeMW = dto.UnitSizeMW,
                DUID = dto.DUID,
                DispatchTypeEnum = dto.DispatchTypeEnum
            };

            return obj;
        }
    }
    
}
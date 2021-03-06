using NemTracker.Dtos.Aemo;
using NemTracker.Dtos.Stations;
using Oxygen.Features;
using Oxygen.Guards;

namespace NemTracker.Model.Model.Stations
{
    public class Station : Entity<long>
    {
        
        // ReSharper disable InconsistentNaming
        public long ParticipantId { get; private set; }
        public string StationName { get; private set; }
        public RegionEnum Region { get; private set; }
        public TechnologyTypeEnum TechnologyType { get; private set; }
        public TechnologyTypeDescriptorEnum TechnologyTypeDescriptor { get; private set; }
        public int PhysicalUnitMin { get; private set; }
        public int PhysicalUnitMax { get; private set; }
        public double UnitSizeMW { get; private set; } = 0;
        public string DUID { get; private set; }
        public DispatchTypeEnum DispatchType { get; private set; } = DispatchTypeEnum.Undefined;
        
        
        public static Station Create(StationDto dto)
        {
            
            Guard.Against(dto.DUID,GuardType.NullOrEmpty);
            
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
                DispatchType = dto.DispatchType
            };

            return obj;
        }

        public void Update(StationDto dto)
        {
            ParticipantId = dto.ParticipantId;
            StationName = dto.StationName;
            Region = dto.Region;
            TechnologyType = dto.TechnologyType;
            TechnologyTypeDescriptor = dto.TechnologyTypeDescriptor;
            PhysicalUnitMin = dto.PhysicalUnitMin;
            PhysicalUnitMax = dto.PhysicalUnitMax;
            UnitSizeMW = dto.UnitSizeMW;
            DUID = dto.DUID;
            DispatchType = dto.DispatchType;
        }

        public StationDto GetDto()
        {
            var dto = new StationDto()
            {
                ParticipantId = ParticipantId,
                StationName = StationName,
                Region = Region,
                TechnologyType = TechnologyType,
                TechnologyTypeDescriptor = TechnologyTypeDescriptor,
                PhysicalUnitMin = PhysicalUnitMin,
                PhysicalUnitMax = PhysicalUnitMax,
                UnitSizeMW = UnitSizeMW,
                DUID = DUID,
                DispatchType = DispatchType
            };

            return dto;

        }
        
    }
    
}
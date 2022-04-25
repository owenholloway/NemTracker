using NemTracker.Dtos.Stations;
using Oxygen.Features;

namespace NemTracker.Model.Model.Stations
{
    public class Participant : Entity<long>
    {
        public string Name { get; private set; }
        public string ABN { get; private set; }
        public bool DemandResponseServiceProviderAncillaryServiceLoad { get; private set; }
        public bool DemandResponseServiceProviderWholesaleDemandResponseUnit { get; private set; }
        public bool GeneratorMarketScheduled { get; private set; }
        public bool GeneratorMarketNonScheduled { get; private set; }
        public bool GeneratorMarketSemiScheduled { get; private set; }
        public bool GeneratorNonMarketScheduled { get; private set; }
        public bool GeneratorNonMarketNonScheduled { get; private set; }
        public bool GeneratorNonMarketSemiScheduled { get; private set; }
        public bool MarketSmallGenerationAggregator { get; private set; }
        public bool MarketCustomer { get; private set; }
        public bool MeteringCoordinator { get; private set; }
        public bool MarketNSP { get; private set; }
        public bool NSPTransmission { get; private set; }
        public bool NSPDistribution { get; private set; }
        public bool NSPOther { get; private set; }
        public bool SpecialParticipantSystemOperator { get; private set; }
        public bool SpecialParticipantDistributionOperator { get; private set; }
        public bool Trader { get; private set; }
        public bool Intending { get; private set; }
        public bool Reallocator { get; private set; }

        public static Participant Create(ParticipantDto dto)
        {
            var obj = new Participant();
            obj.Update(dto);
            return obj;
        }

        public void Update(ParticipantDto dto)
        {
            Name = dto.Name;
            ABN = dto.ABN;
            DemandResponseServiceProviderAncillaryServiceLoad 
                = dto.DemandResponseServiceProviderAncillaryServiceLoad;
            DemandResponseServiceProviderWholesaleDemandResponseUnit 
                = dto.DemandResponseServiceProviderWholesaleDemandResponseUnit;
            GeneratorMarketScheduled = dto.GeneratorMarketScheduled;
            GeneratorMarketNonScheduled = dto.GeneratorMarketNonScheduled;
            GeneratorMarketSemiScheduled = dto.GeneratorMarketSemiScheduled;
            GeneratorNonMarketScheduled = dto.GeneratorNonMarketScheduled;
            GeneratorNonMarketNonScheduled = dto.GeneratorNonMarketNonScheduled;
            GeneratorNonMarketSemiScheduled = dto.GeneratorNonMarketSemiScheduled;
            MarketSmallGenerationAggregator = dto.MarketSmallGenerationAggregator;
            MarketCustomer = dto.MarketCustomer;
            MeteringCoordinator = dto.MeteringCoordinator;
            MarketNSP = dto.MarketNSP;
            NSPTransmission = dto.NSPTransmission;
            NSPDistribution = dto.NSPDistribution;
            NSPOther = dto.NSPOther;
            SpecialParticipantSystemOperator = dto.SpecialParticipantSystemOperator;
            SpecialParticipantDistributionOperator = dto.SpecialParticipantDistributionOperator;
            Trader = dto.Trader;
            Intending = dto.Intending;
            Reallocator = dto.Reallocator;
        }
        
    }
}
using System;

namespace NemTracker.Dtos.Stations
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ABN { get; set; }
        public bool DemandResponseServiceProviderAncillaryServiceLoad { get; set; }
        public bool DemandResponseServiceProviderWholesaleDemandResponseUnit { get; set; }
        public bool GeneratorMarketScheduled { get; set; }
        public bool GeneratorMarketNonScheduled { get; set; }
        public bool GeneratorMarketSemiScheduled { get; set; }
        public bool GeneratorNonMarketScheduled { get; set; }
        public bool GeneratorNonMarketNonScheduled { get; set; }
        public bool GeneratorNonMarketSemiScheduled { get; set; }
        public bool MarketSmallGenerationAggregator { get; set; }
        public bool MarketCustomer { get; set; }
        public bool MeteringCoordinator { get; set; }
        public bool MarketNSP { get; set; }
        public bool NSPTransmission { get; set; }
        public bool NSPDistribution { get; set; }
        public bool NSPOther { get; set; }
        public bool SpecialParticipantSystemOperator { get; set; }
        public bool SpecialParticipantDistributionOperator { get; set; }
        public bool Trader { get; set; }
        public bool Intending { get; set; }
        public bool Reallocator { get; set; }

    }
}
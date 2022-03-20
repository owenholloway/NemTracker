using System;
using NemTracker.Dtos.Stations;
using Oxygen.Features;

namespace NemTracker.Model.P5Minute
{
    public class RegionSolution : Entity<long>
    {
        public DateTime RunTime { get; private set; }
        public DateTime Interval { get; private set; }
        public RegionEnum Region { get; private set; }
        //Data Values
        public double Rrp { get; private set; }
        public double Rop { get; private set; }
        public double ExcessGeneration { get; private set; }
        public double Raise6SecRrp { get; private set; }
        public double Raise6SecRop { get; private set; }
        public double Raise60SecRrp { get; private set; }
        public double Raise60SecRop { get; private set; }
        public double Raise5MinRrp { get; private set; }
        public double Raise5MinRop { get; private set; }
        public double RaiseRegRrp { get; private set; }
        public double RaiseRegRop { get; private set; }
        public double Lower6SecRrp { get; private set; }
        public double Lower6SecRop { get; private set; }
        public double Lower60SecRrp { get; private set; }
        public double Lower60SecRop { get; private set; }
        public double Lower5MinRrp { get; private set; }
        public double Lower5MinRop { get; private set; }
        public double LowerRegRrp { get; private set; }
        public double LowerRegRop { get; private set; }
        public double TotalDemand { get; private set; }
        public double AvailableGeneration { get; private set; }
        public double AvailableLoad { get; private set; }
        public double DemandForecast { get; private set; }
        public double DispatchableGeneration { get; private set; }
        public double DispatchableLoad { get; private set; }
        public double NetInterchange { get; private set; }
        public double Lower5MinDispatch { get; private set; }
        public double Lower5MinImport { get; private set; }
        public double Lower5MinLocalDispatch { get; private set; }
        public double Lower5MinLocalReq { get; private set; }
        public double Lower5MinReq { get; private set; }
        public double Lower60SecDispatch { get; private set; }
        public double Lower60SecImport { get; private set; }
        public double Lower60SecLocalDispatch { get; private set; }
        public double Lower60SecLocalReq { get; private set; }
        public double Lower60SecReq { get; private set; }
        public double Lower6SecDispatch { get; private set; }
        public double Lower6SecImport { get; private set; }
        public double Lower6SecLocalDispatch { get; private set; }
        public double Lower6SecLocalReq { get; private set; }
        public double Lower6SecReq { get; private set; }
        public double Raise5MinDispatch { get; private set; }
        public double Raise5MinImport { get; private set; }
        public double Raise5MinLocalDispatch { get; private set; }
        public double Raise5MinLocalReq { get; private set; }
        public double Raise5MinReq { get; private set; }
        public double Raise60SecDispatch { get; private set; }
        public double Raise60SecImport { get; private set; }
        public double Raise60SecLocalDispatch { get; private set; }
        public double Raise60SecLocalReq { get; private set; }
        public double Raise60SecReq { get; private set; }
        public double Raise6SecDispatch { get; private set; }
        public double Raise6SecImport { get; private set; }
        public double Raise6SecLocalDispatch { get; private set; }
        public double Raise6SecLocalReq { get; private set; }
        public double Raise6SecReq { get; private set; }
        public double AggregateDispatchError { get; private set; }
        public double InitialSupply { get; private set; }
        public double ClearedSupply { get; private set; }
        public double LowerRegImport { get; private set; }
        public double LowerRegDispatch { get; private set; }
        public double LowerRegLocalDispatch { get; private set; }
        public double LowerRegLocalReq { get; private set; }
        public double LowerRegReq { get; private set; }
        public double RaiseRegImport { get; private set; }
        public double RaiseRegDispatch { get; private set; }
        public double RaiseRegLocalDispatch { get; private set; }
        public double RaiseRegLocalReq { get; private set; }
        public double RaiseRegReq { get; private set; }
        public double Raise5MinLocalViolation { get; private set; }
        public double RaiseRegLocalViolation { get; private set; }
        public double Raise60SecLocalViolation { get; private set; }
        public double Raise6SecLocalViolation { get; private set; }
        public double Lower5MinLocalViolation { get; private set; }
        public double LowerRegLocalViolation { get; private set; }
        public double Lower60SecLocalViolation { get; private set; }
        public double Lower6SecLocalViolation { get; private set; }
        public double Raise5MinViolation { get; private set; }
        public double RaiseRegViolation { get; private set; }
        public double Raise60SecViolation { get; private set; }
        public double Raise6SecViolation { get; private set; }
        public double Lower5MinViolation { get; private set; }
        public double LowerRegViolation { get; private set; }
        public double Lower60SecViolation { get; private set; }
        public double Lower6SecViolation { get; private set; }
        public DateTime LastChanged { get; private set; }
        public double TotalIntermittentGeneration { get; private set; }
        public double DemandAndNonSchedgen { get; private set; }
        public double Uigf { get; private set; }
        public double SemiScheduleClearedMw { get; private set; }
        public double SemiScheduleComplianceMw { get; private set; }
        public double SsSolarUigf { get; private set; }
        public double SsWindUigf { get; private set; }
        public double SsSolarClearedMw { get; private set; }
        public double SsWindClearedMw { get; private set; }
        public double SsSolarComplianceMw { get; private set; }
        public double SsWindComplianceMw { get; private set; }
        public double WdrInitialMw { get; private set; }
        public double WdrAvailable { get; private set; }
        public double WdrDispatched { get; private set; }

    }

}
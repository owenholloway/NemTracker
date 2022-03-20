using System;
using NemTracker.Dtos.Stations;

namespace NemTracker.Dtos.P5Minute
{
    public class RegionSolutionDto
    {
        public long Id { get; set; }
        public DateTime RunTime { get; set; }
        public DateTime Interval { get; set; }
        public RegionEnum Region { get; set; }
        //Data Values
        public double Rrp { get; set; } = 0;
        public double Rop { get; set; } = 0;
        public double ExcessGeneration { get; set; } = 0;
        public double Raise6SecRrp { get; set; } = 0;
        public double Raise6SecRop { get; set; } = 0;
        public double Raise60SecRrp { get; set; } = 0;
        public double Raise60SecRop { get; set; } = 0;
        public double Raise5MinRrp { get; set; } = 0;
        public double Raise5MinRop { get; set; } = 0;
        public double RaiseRegRrp { get; set; } = 0;
        public double RaiseRegRop { get; set; } = 0;
        public double Lower6SecRrp { get; set; } = 0;
        public double Lower6SecRop { get; set; } = 0;
        public double Lower60SecRrp { get; set; } = 0;
        public double Lower60SecRop { get; set; } = 0;
        public double Lower5MinRrp { get; set; } = 0;
        public double Lower5MinRop { get; set; } = 0;
        public double LowerRegRrp { get; set; } = 0;
        public double LowerRegRop { get; set; } = 0;
        public double Totaldemand { get; set; } = 0;
        public double AvailableGeneration { get; set; } = 0;
        public double AvailableLoad { get; set; } = 0;
        public double DemandForecast { get; set; } = 0;
        public double DispatchableGeneration { get; set; } = 0;
        public double DispatchableLoad { get; set; } = 0;
        public double NetinterChange { get; set; } = 0;
        public double Lower5MinDispatch { get; set; } = 0;
        public double Lower5MinImport { get; set; } = 0;
        public double Lower5MinLocalDispatch { get; set; } = 0;
        public double Lower5MinLocalReq { get; set; } = 0;
        public double Lower5MinReq { get; set; } = 0;
        public double Lower60SecDispatch { get; set; } = 0;
        public double Lower60SecImport { get; set; } = 0;
        public double Lower60SecLocalDispatch { get; set; } = 0;
        public double Lower60SecLocalReq { get; set; } = 0;
        public double Lower60SecReq { get; set; } = 0;
        public double Lower6SecDispatch { get; set; } = 0;
        public double Lower6SecImport { get; set; } = 0;
        public double Lower6SecLocalDispatch { get; set; } = 0;
        public double Lower6SecLocalReq { get; set; } = 0;
        public double Lower6SecReq { get; set; } = 0;
        public double Raise5MinDispatch { get; set; } = 0;
        public double Raise5MinImport { get; set; } = 0;
        public double Raise5MinLocalDispatch { get; set; } = 0;
        public double Raise5MinLocalReq { get; set; } = 0;
        public double Raise5MinReq { get; set; } = 0;
        public double Raise60SecDispatch { get; set; } = 0;
        public double Raise60SecImport { get; set; } = 0;
        public double Raise60SecLocalDispatch { get; set; } = 0;
        public double Raise60SecLocalReq { get; set; } = 0;
        public double Raise60SecReq { get; set; } = 0;
        public double Raise6SecDispatch { get; set; } = 0;
        public double Raise6SecImport { get; set; } = 0;
        public double Raise6SecLocalDispatch { get; set; } = 0;
        public double Raise6SecLocalReq { get; set; } = 0;
        public double Raise6SecReq { get; set; } = 0;
        public double AggregateDispatchError { get; set; } = 0;
        public double InitialSupply { get; set; } = 0;
        public double ClearedSupply { get; set; } = 0;
        public double LowerRegImport { get; set; } = 0;
        public double LowerRegDispatch { get; set; } = 0;
        public double LowerRegLocalDispatch { get; set; } = 0;
        public double LowerRegLocalReq { get; set; } = 0;
        public double LowerRegReq { get; set; } = 0;
        public double RaiseRegImport { get; set; } = 0;
        public double RaiseRegDispatch { get; set; } = 0;
        public double RaiseRegLocalDispatch { get; set; } = 0;
        public double RaiseRegLocalReq { get; set; } = 0;
        public double RaiseRegReq { get; set; } = 0;
        public double Raise5MinLocalViolation { get; set; } = 0;
        public double RaiseRegLocalViolation { get; set; } = 0;
        public double Raise60SecLocalViolation { get; set; } = 0;
        public double Raise6SecLocalViolation { get; set; } = 0;
        public double Lower5MinLocalViolation { get; set; } = 0;
        public double LowerRegLocalViolation { get; set; } = 0;
        public double Lower60SecLocalViolation { get; set; } = 0;
        public double Lower6SecLocalViolation { get; set; } = 0;
        public double Raise5MinViolation { get; set; } = 0;
        public double RaiseRegViolation { get; set; } = 0;
        public double Raise60SecViolation { get; set; } = 0;
        public double Raise6SecViolation { get; set; } = 0;
        public double Lower5MinViolation { get; set; } = 0;
        public double LowerRegViolation { get; set; } = 0;
        public double Lower60SecViolation { get; set; } = 0;
        public double Lower6SecViolation { get; set; } = 0;
        public DateTime LastChanged { get; set; }
        public double TotalIntermittentGeneration { get; set; } = 0;
        public double DemandAndNonSchedgen { get; set; } = 0;
        public double Uigf { get; set; } = 0;
        public double SemiScheduleClearedMw { get; set; } = 0;
        public double SemiScheduleComplianceMw { get; set; } = 0;
        public double SsSolarUigf { get; set; } = 0;
        public double SsWindUigf { get; set; } = 0;
        public double SsSolarClearedMw { get; set; } = 0;
        public double SsWindClearedMw { get; set; } = 0;
        public double SsSolarComplianceMw { get; set; } = 0;
        public double SsWindComplianceMw { get; set; } = 0;
        public double WdrInitialMw { get; set; } = 0;
        public double WdrAvailable { get; set; } = 0;
        public double WdrDispatched { get; set; } = 0;
    }
}
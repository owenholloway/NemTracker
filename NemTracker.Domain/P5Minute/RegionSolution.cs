using System;
using NemTracker.Dtos.P5Minute;
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

        public static RegionSolution Create(RegionSolutionDto dto)
        {
            var obj = new RegionSolution();

            obj.Update(dto);
            
            return obj;
        }

        public void Update(RegionSolutionDto dto)
        {
            RunTime = dto.RunTime;
            Interval = dto.Interval;
            Region = dto.Region;
            Rrp = dto.Rrp;
            Rop = dto.Rop;
            ExcessGeneration = dto.ExcessGeneration;
            Raise6SecRrp = dto.Raise6SecRrp;
            Raise6SecRop = dto.Raise6SecRop;
            Raise60SecRrp = dto.Raise60SecRrp;
            Raise60SecRop = dto.Raise60SecRop;
            Raise5MinRrp = dto.Raise5MinRrp;
            Raise5MinRop = dto.Raise5MinRop;
            RaiseRegRrp = dto.RaiseRegRrp;
            RaiseRegRop = dto.RaiseRegRop;
            Lower6SecRrp = dto.Lower6SecRrp;
            Lower6SecRop = dto.Lower6SecRop;
            Lower60SecRrp = dto.Lower60SecRrp;
            Lower60SecRop = dto.Lower60SecRop;
            Lower5MinRrp = dto.Lower5MinRrp;
            Lower5MinRop = dto.Lower5MinRop;
            LowerRegRrp = dto.LowerRegRrp;
            LowerRegRop = dto.LowerRegRop;
            TotalDemand = dto.TotalDemand;
            AvailableGeneration = dto.AvailableGeneration;
            AvailableLoad = dto.AvailableLoad;
            DemandForecast = dto.DemandForecast;
            DispatchableGeneration = dto.DispatchableGeneration;
            DispatchableLoad = dto.DispatchableLoad;
            NetInterchange = dto.NetInterchange;
            Lower5MinDispatch = dto.Lower5MinDispatch;
            Lower5MinImport = dto.Lower5MinImport;
            Lower5MinLocalDispatch = dto.Lower5MinLocalDispatch;
            Lower5MinLocalReq = dto.Lower5MinLocalReq;
            Lower5MinReq = dto.Lower5MinReq;
            Lower60SecDispatch = dto.Lower60SecDispatch;
            Lower60SecImport = dto.Lower60SecImport;
            Lower60SecLocalDispatch = dto.Lower60SecLocalDispatch;
            Lower60SecLocalReq = dto.Lower60SecLocalReq;
            Lower60SecReq = dto.Lower60SecReq;
            Lower6SecDispatch = dto.Lower6SecDispatch;
            Lower6SecImport = dto.Lower6SecImport;
            Lower6SecLocalDispatch = dto.Lower6SecLocalDispatch;
            Lower6SecLocalReq = dto.Lower6SecLocalReq;
            Lower6SecReq = dto.Lower6SecReq;
            Raise5MinDispatch = dto.Raise5MinDispatch;
            Raise5MinImport = dto.Raise5MinImport;
            Raise5MinLocalDispatch = dto.Raise5MinLocalDispatch;
            Raise5MinLocalReq = dto.Raise5MinLocalReq;
            Raise5MinReq = dto.Raise5MinReq;
            Raise60SecDispatch = dto.Raise60SecDispatch;
            Raise60SecImport = dto.Raise60SecImport;
            Raise60SecLocalDispatch = dto.Raise60SecLocalDispatch;
            Raise60SecLocalReq = dto.Raise60SecLocalReq;
            Raise60SecReq = dto.Raise60SecReq;
            Raise6SecDispatch = dto.Raise6SecDispatch;
            Raise6SecImport = dto.Raise6SecImport;
            Raise6SecLocalDispatch = dto.Raise6SecLocalDispatch;
            Raise6SecLocalReq = dto.Raise6SecLocalReq;
            Raise6SecReq = dto.Raise6SecReq;
            AggregateDispatchError = dto.AggregateDispatchError;
            InitialSupply = dto.InitialSupply;
            ClearedSupply = dto.ClearedSupply;
            LowerRegImport = dto.LowerRegImport;
            LowerRegDispatch = dto.LowerRegDispatch;
            LowerRegLocalDispatch = dto.LowerRegLocalDispatch;
            LowerRegLocalReq = dto.LowerRegLocalReq;
            LowerRegReq = dto.LowerRegReq;
            RaiseRegImport = dto.RaiseRegImport;
            RaiseRegDispatch = dto.RaiseRegDispatch;
            RaiseRegLocalDispatch = dto.RaiseRegLocalDispatch;
            RaiseRegLocalReq = dto.RaiseRegLocalReq;
            RaiseRegReq = dto.RaiseRegReq;
            Raise5MinLocalViolation = dto.Raise5MinLocalViolation;
            RaiseRegLocalViolation = dto.RaiseRegLocalViolation;
            Raise60SecLocalViolation = dto.Raise60SecLocalViolation;
            Raise6SecLocalViolation = dto.Raise6SecLocalViolation;
            Lower5MinLocalViolation = dto.Lower5MinLocalViolation;
            LowerRegLocalViolation = dto.LowerRegLocalViolation;
            Lower60SecLocalViolation = dto.Lower60SecLocalViolation;
            Lower6SecLocalViolation = dto.Lower6SecLocalViolation;
            Raise5MinViolation = dto.Raise5MinViolation;
            RaiseRegViolation = dto.RaiseRegViolation;
            Raise60SecViolation = dto.Raise60SecViolation;
            Raise6SecViolation = dto.Raise6SecViolation;
            Lower5MinViolation = dto.Lower5MinViolation;
            LowerRegViolation = dto.LowerRegViolation;
            Lower60SecViolation = dto.Lower60SecViolation;
            Lower6SecViolation = dto.Lower6SecViolation;
            LastChanged = dto.LastChanged;
            TotalIntermittentGeneration = dto.TotalIntermittentGeneration;
            DemandAndNonSchedgen = dto.DemandAndNonSchedgen;
            Uigf = dto.Uigf;
            SemiScheduleClearedMw = dto.SemiScheduleClearedMw;
            SemiScheduleComplianceMw = dto.SemiScheduleComplianceMw;
            SsSolarUigf = dto.SsSolarUigf;
            SsWindUigf = dto.SsWindUigf;
            SsSolarClearedMw = dto.SsSolarClearedMw;
            SsWindClearedMw = dto.SsWindClearedMw;
            SsSolarComplianceMw = dto.SsSolarComplianceMw;
            SsWindComplianceMw = dto.SsWindComplianceMw;
            WdrInitialMw = dto.WdrInitialMw;
            WdrAvailable = dto.WdrAvailable;
            WdrDispatched = dto.WdrDispatched;
        }
        
    }

}
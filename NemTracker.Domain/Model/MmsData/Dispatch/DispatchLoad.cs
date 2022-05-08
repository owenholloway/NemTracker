using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oxygen.Features;

// This is a read only record therefore access is private for all
// ReSharper disable UnusedAutoPropertyAccessor.Local



namespace NemTracker.Model.Model.MmsData.Dispatch;

public class DispatchLoad : Entity<long>
{
    [Column("settlementdate")]
    public DateTime SettlementDate { get; private set; }
    [Column("runno")]
    public int RunNo { get; private set; }
    [Column("duid")]
    public string Duid { get; private set; }
    [Column("dispatchinterval")]
    public long DispatchInterval { get; private set; }
    [Column("lastchanged")]
    public DateTime LastChanged { get; private set; }
    
    [Column("initialmw")]
    public int InitialMw { get; private set; }
    [Column("totalcleared")]
    public int TotalCleared { get; private set; }
    [Column("rampdownrate")]
    public int RampdownRate { get; private set; }
    [Column("rampuprate")]
    public int RampUpRate { get; private set; }
    [Column("lower5min")]
    public int Lower5Min { get; private set; }
    [Column("lower60sec")]
    public int Lower60Sec { get; private set; }
    [Column("lower6sec")]
    public int Lower6Sec { get; private set; }
    [Column("raise5min")]
    public int Raise5Min { get; private set; }
    [Column("raise60sec")]
    public int Raise60Sec { get; private set; }
    [Column("raise6sec")]
    public int Raise6Sec { get; private set; }
    [Column("downepf")]
    public int? DownEpf { get; private set; }
    [Column("upepf")]
    public int? UpEpf { get; private set; }
    [Column("marginal5minvalue")]
    public int? Marginal5MinValue { get; private set; }
    [Column("marginal60secvalue")]
    public int? Marginal60SecValue { get; private set; }
    [Column("marginal6secvalue")]
    public int? Marginal6SecValue { get; private set; }
    [Column("marginalvalue")]
    public int? MarginalValue { get; private set; }
    [Column("violation5mindegree")]
    public int? Violation5MinDegree { get; private set; }
    [Column("violation60secdegree")]
    public int? Violation60SecDegree { get; private set; }
    [Column("violation6secdegree")]
    public int? Violation6SecDegree { get; private set; }
    [Column("violationdegree")]
    public int? ViolationDegree { get; private set; }
    
}
using System.ComponentModel;

namespace NemTracker.Dtos
{
    public enum TechnologyTypeDescriptorEnum
    {
        [Description("Undefined")]
        Undefined = -1,
        [Description("Battery and Inverter")]
        BatteryandInverter = 1,
        [Description("Run of River")]
        RunofRiver = 2,
        [Description("Photovoltaic Flat panel")]
        PhotovoltaicFlatpanel = 3,
        [Description("Photovoltaic Tracking Flat panel")]
        PhotovoltaicTrackingFlatpanel = 4,
        [Description("Compression Reciprocating Engine")]
        CompressionReciprocatingEngine = 5,
        [Description("Spark Ignition Reciprocating Engine")]
        SparkIgnitionReciprocatingEngine = 6,
        [Description("Wind - Onshore")]
        WindOnshore = 7,
        [Description("Open Cycle Gas turbines (OCGT)")]
        OpenCycleGasturbines = 8,
        [Description("Battery")]
        Battery = 9,
        [Description("Hydro - Gravity")]
        HydroGravity = 10,
        [Description("Combined Cycle Gas Turbine (CCGT)")]
        CombinedCycleGasTurbine = 11,
        [Description("Steam Sub-Critical")]
        SteamSubCritical = 12,
        [Description("Steam Super Critical")]
        SteamSuperCritical = 13,
        [Description("Photovoltaic Tracking  Flat Panel")]
        PhotovoltaicTrackingFlatPanel = 14,
        [Description("Pump Storage")]
        PumpStorage = 15
    }
}
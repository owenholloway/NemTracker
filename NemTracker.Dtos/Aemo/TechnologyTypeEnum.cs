using System.ComponentModel;

namespace NemTracker.Dtos.Aemo
{
    public enum TechnologyTypeEnum : sbyte
    {
        [Description("Undefined")]
        Undefined = -1,
        [Description("Storage")]
        Storage = 1,
        [Description("Renewable")]
        Renewable = 2,
        [Description("Combustion")]
        Combustion = 3
    }
}
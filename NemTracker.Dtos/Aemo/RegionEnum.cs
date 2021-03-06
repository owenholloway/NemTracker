// ReSharper disable InconsistentNaming
namespace NemTracker.Dtos.Aemo
{
    /*
     * Regions,
     * these have been broken up by postcode for no, ie. Tas1 is 7000 so Region ID is 7
     */   
    public enum RegionEnum : sbyte
    {
        UNDF = -1,
        NSW1 = 2,
        VIC1 = 3,
        QLD1 = 4,
        SA1  = 5,
        TAS1 = 7,
    }
}
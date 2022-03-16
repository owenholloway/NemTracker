// ReSharper disable InconsistentNaming
namespace NemTracker.Dtos.Stations
{
    /*
     * Regions,
     * these have been broken up by postcode for no, ie. Tas1 is 7000 so Region ID is 7
     */   
    public enum RegionEnum : sbyte
    {
        NSW1 = 2,
        VIC1 = 3,
        OLD1 = 4,
        SA1  = 5,
        TAS1 = 7,
    }
}
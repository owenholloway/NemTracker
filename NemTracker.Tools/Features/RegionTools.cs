using NemTracker.Dtos.Aemo;

namespace NemTracker.Tools.Features
{
    public static class RegionTools
    {
        public static RegionEnum GetRegion(this string value)
        {
            if (value.Contains(RegionEnum.NSW1.GetDescription()))
            {
                return RegionEnum.NSW1;
            }
            
            if (value.Contains(RegionEnum.VIC1.GetDescription()))
            {
                return RegionEnum.VIC1;
            }
            
            if (value.Contains(RegionEnum.QLD1.GetDescription()))
            {
                return RegionEnum.QLD1;
            }
            
            if (value.Contains(RegionEnum.SA1.GetDescription()))
            {
                return RegionEnum.SA1;
            }
            
            if (value.Contains(RegionEnum.TAS1.GetDescription()))
            {
                return RegionEnum.TAS1;
            }

            return RegionEnum.UNDF;
        }
    }
}
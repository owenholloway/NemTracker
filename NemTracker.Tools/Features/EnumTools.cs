using System;
using System.ComponentModel;
using System.Linq;
using NemTracker.Dtos.Aemo;

namespace NemTracker.Tools.Features
{
    public static class EnumTools
    {
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes 
                && attributes.Any())
            {
                return attributes.First().Description;
            }
            
            return value.ToString();
        }

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
using System;
using System.ComponentModel;
using System.Linq;

namespace NemTracker.Tools.Features
{
    public static class EnumTools
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }
            
            return value.ToString();
        }
    }
}
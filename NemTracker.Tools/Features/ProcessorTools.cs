using System;

namespace NemTracker.Tools.Features
{
    public static class ProcessorTools
    {
        public static double DoubleValue(this string value)
        {
            if (value.Trim().Length == 0)
            {
                return 0.0;
            }
            
            return Double.Parse(value.Trim());
        }
    }
}
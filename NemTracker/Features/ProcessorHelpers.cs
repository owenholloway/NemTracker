using System;

namespace NemTracker.Features
{
    public static class ProcessorHelpers
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
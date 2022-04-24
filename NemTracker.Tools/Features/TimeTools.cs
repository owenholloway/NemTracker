using System;
using System.Globalization;

namespace NemTracker.Tools.Features
{
    public static class TimeTools
    {
        public static long NextNemInterval(this DateTime dateTime)
        {

            dateTime = dateTime.AddSeconds( -1 * dateTime.Second);
            dateTime = dateTime.AddMilliseconds( -1 * dateTime.Millisecond);
            
            if (dateTime.Minute > 55)
            {
                var minutes = dateTime.Minute;
                var minuteDelta = 60 - minutes;
                var secondDelta = minuteDelta * 60;
                dateTime = dateTime.AddSeconds(secondDelta);
            }
            else
            {
                var minuteDelta = 5 - (dateTime.Minute % 5);
                var secondDelta = minuteDelta * 60;
                dateTime = dateTime.AddSeconds(secondDelta);
            }
            
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = dateTime.Day;
            var hour = dateTime.Hour;
            var minute = dateTime.Minute;

            var result = year.ToString("0000") +
                         month.ToString("00") +
                         day.ToString("00") +
                         hour.ToString("00") +
                         minute.ToString("00");

            return Int64.Parse(result);
        }
        
        
        public static DateTime GetDateTime(this string value, string dateTimeFormat)
        {
            return DateTime.ParseExact(value.Replace("\"",""), 
                dateTimeFormat, new CultureInfo("En-AU"));
        }

        public static DateTime GetDateTime(this long value, string dateTimeFormat)
        {
            return value.ToString().GetDateTime(dateTimeFormat);
        }
        
        /*
        public static int ThisNemInterval(this DateTime dateTime)
        {
            
        }*/
        
    }
}
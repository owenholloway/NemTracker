using System;
using NemTracker.Features;

namespace NemTracker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var nemProcessor = new NemRegistrationsProcessor();
            var stations = nemProcessor.GetStations();

            foreach (var station in stations)
            {
                Console.Write("Station: ");
                Console.Write(station.StationName);
                Console.Write(", ");
                
                Console.Write("Units Min: ");
                Console.Write(station.PhysicalUnitMin);
                Console.Write(", ");
                
                Console.Write("Units Max: ");
                Console.Write(station.PhysicalUnitMax);
                Console.WriteLine();
            }

        }
    }
}
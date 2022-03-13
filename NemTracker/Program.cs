using System;
using NemTracker.Features;

namespace NemTracker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var nemProcessor = new NemRegistrationsProcessor();
            var station = nemProcessor.GetStations();

        }
    }
}
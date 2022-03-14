using System;
using System.Linq;
using System.Reflection;

namespace NemTracker.Services
{
    public class AppScanner
    {
        public static Assembly[] GetAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            var oxygenAssemblies = assemblies.Where(a => a.FullName.Contains("Oxygen")).ToArray();
            
            var nemTrackerAssemblies = assemblies.Where(a => a.FullName.Contains("Oxygen")).ToArray();

            var allAssemblies = assemblies
                .Concat(oxygenAssemblies).ToArray()
                .Concat(nemTrackerAssemblies).ToArray();

            return allAssemblies;
        }

    }
}
using System;

namespace NemTracker.Services
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
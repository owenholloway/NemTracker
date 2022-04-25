using System;

namespace NemTracker.Features
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
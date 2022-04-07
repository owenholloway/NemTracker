using System.ComponentModel;

namespace NemTracker.Dtos.Aemo
{
    public enum ClassificationEnum
    {        
        [Description("Undefined")]
        Undefined = -1,
        Scheduled = 1,
        NonScheduled = 2,
        SemiScheduled = 3
    }
}
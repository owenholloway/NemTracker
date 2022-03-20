using System;

namespace NemTracker.Dtos.P5Minute
{
    public class IntervalDto
    {
        public long P5MinNumber { get; set; }
        public bool Processed { get; set; }
        public AemoTypeEnum IntervalDataType { get; set; }
    }
}
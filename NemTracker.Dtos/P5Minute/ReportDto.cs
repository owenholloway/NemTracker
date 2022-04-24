using System;

namespace NemTracker.Dtos.P5Minute
{
    public class ReportDto
    {
        public DateTime PublishDateTime { get; set; }
        public DateTime IntervalDateTime { get; set; }
        public string Path { get; set; }
        public bool Processed { get; set; }
        public IntervalProcessTypeEnum IntervalProcessType { get; set; }
    }
}
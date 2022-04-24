using System;
using NemTracker.Dtos.Reports;
using Oxygen.Features;

namespace NemTracker.Model.Reports
{
    public class Report : Entity<long>
    {
        public DateTime PublishDateTime { get; private set; }
        public DateTime IntervalDateTime { get; private set; }
        public string Path { get; private set; }
        public bool Processed { get; private set; }
        public IntervalProcessTypeEnum IntervalProcessType { get; private set; }
        public ReportTypeEnum ReportType { get; private set; }

        public static Report Create(ReportDto dto)
        {
            var obj = new Report();

            obj.Update(dto);
            
            return obj;
        }

        public void Update(ReportDto dto)
        {
            PublishDateTime = dto.PublishDateTime;
            IntervalDateTime = dto.IntervalDateTime;
            Path = dto.Path;
            Processed = dto.Processed;
            IntervalProcessType = dto.IntervalProcessType;
            ReportType = dto.ReportType;
        }
        
    }
}
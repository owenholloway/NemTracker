using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NemTracker.Dtos.Reports;
using NemTracker.Features;
using NemTracker.Features.Ingest.Reports;
using NemTracker.Features.Tools;
using NemTracker.Model.Model.Reports;
using NemTracker.Model.Observables;
using NemTracker.Persistence.Features;
using NemTracker.Persistence.Features.NemTrackerData;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class ReportIngestService : IHostedService
    {
        
        private DateTime _nextRun;
        private DateTime _lastPeriodProcessed;

        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;

        private static ReportHandler _reportHandler;
        private static P5MinIngestObserver _p5MinIngestObserver;
        
        public ReportIngestService(IConfiguration configuration)
        {
            _lastPeriodProcessed = new DateTime();
            
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(configuration.GetValue<string>("APPLICATION_DATABASE"));
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            var nemdbContext = new NEMTrackerContext(optionsBuilder.Options);
            _readOnlyRepository = new ReadOnlyRepository(nemdbContext);
            _readWriteRepository = new ReadWriteRepository(nemdbContext);

            _reportHandler = new ReportHandler();
            _p5MinIngestObserver = new P5MinIngestObserver(configuration);
            _p5MinIngestObserver.Subscribe(_reportHandler);

        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var processP5DataTask = ProcessReportData();
                    await processP5DataTask;
                    processP5DataTask.Dispose();
                    _readWriteRepository.Commit();
                    _nextRun = DateTime.Now;
                    _nextRun = _nextRun.AddSeconds(10);
                    
                    await Task.Delay(UntilNextExecution(), cancellationToken);
                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task ProcessReportData()
        {
            return Task.Run(() =>
            {
                var reports = P5ReportProcessor.CheckNewInstructions();

                var newReportExists = reports.Any(r => r.IntervalDateTime > _lastPeriodProcessed);

                if (!newReportExists) return;
                
                foreach (var reportDto in reports)
                {
                    if (_readOnlyRepository.Table<Report, long>()
                        .Any(r => r.IntervalDateTime.Equals(reportDto.IntervalDateTime)
                                   && r.PublishDateTime.Equals(reportDto.PublishDateTime))) continue;

                    var report = Report.Create(reportDto);
                    _readWriteRepository.Create<Report, long>(report);

                    if (reportDto.IntervalProcessType != IntervalProcessTypeEnum.Realtime) continue;
                    _reportHandler.ReportToBeConsumed(reportDto);
                    report.MarkProcessed();

                }

            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
    }
}
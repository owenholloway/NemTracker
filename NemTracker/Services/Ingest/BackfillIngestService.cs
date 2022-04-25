using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NemTracker.Dtos.Reports;
using NemTracker.Features.Ingest.Reports;
using NemTracker.Features.Tools;
using NemTracker.Model.Model.Reports;
using NemTracker.Model.Observables;
using NemTracker.Persistence.Features;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class BackFillIngestService : IHostedService
    {
        
        private DateTime _nextRun;

        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;

        private static ReportHandler _reportHandler;
        private static P5MinIngestObserver _p5MinIngestObserver;
        
        public const long TicksPerDay = 864000000000;
        
        public BackFillIngestService(IConfiguration configuration)
        {
            
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ApplicationDatabase"));

            var nemdbContext = new NEMDBContext(optionsBuilder.Options);
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
                    _nextRun = _nextRun.AddSeconds(30);
                    
                    await Task.Delay(UntilNextExecution(), cancellationToken);
                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task ProcessReportData()
        {
            return Task.Run(() =>
            {

                if (!_readOnlyRepository.Table<Report, long>()
                    .Any(r => r.Processed == false 
                              && r.IntervalProcessType == IntervalProcessTypeEnum.Historical)) return;

                Console.WriteLine("P5 Backfill Data ingest is starting");
                
                var report = _readWriteRepository.Table<Report, long>()
                    .FirstOrDefault(r => r.Processed == false 
                                         && r.IntervalProcessType == IntervalProcessTypeEnum.Historical);

                if (report?.IntervalDateTime.Ticks - DateTime.Now.Ticks > 2 * TicksPerDay)
                {
                    _readWriteRepository.Delete<Report, long>(report);
                    return;
                }
                
                _reportHandler.ReportToBeConsumed(report?.GetDto());
                
                report?.MarkProcessed();
                
                Console.WriteLine("P5 Backfill Data ingest is complete");

            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
    }
}
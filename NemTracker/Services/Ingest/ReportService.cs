using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using NemTracker.Features;
using NemTracker.Model.Reports;
using NemTracker.Persistence.Features;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class ReportService : IHostedService
    {
        
        private DateTime _nextRun;

        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;
        
        public ReportService(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ApplicationDatabase"));
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            var nemdbContext = new NEMDBContext(optionsBuilder.Options);
            _readOnlyRepository = new ReadOnlyRepository(nemdbContext);
            _readWriteRepository = new ReadWriteRepository(nemdbContext);
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    
                    Console.WriteLine("Report Data ingest is starting");
                    
                    var processP5DataTask = ProcessReportData();
                    await processP5DataTask;
                    processP5DataTask.Dispose();
                    _readWriteRepository.Commit();
                    
                    Console.WriteLine("Report Data ingest is completed");
                    _nextRun = DateTime.Now;
                    _nextRun = _nextRun.AddSeconds(5);
                    
                    await Task.Delay(UntilNextExecution(), cancellationToken);

                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task ProcessReportData()
        {
            return Task.Run(() =>
            {
                var processor = new P5ReportProcessor();
                var reports = P5ReportProcessor.CheckNewInstructions();

                foreach (var reportDto in reports.Where(reportDto => !_readOnlyRepository.Table<Report, long>()
                    .Any(r => r.IntervalDateTime.Equals(reportDto.IntervalDateTime))))
                {
                    _readWriteRepository.Create<Report, long>(Report.Create(reportDto));
                }

            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
    }
}
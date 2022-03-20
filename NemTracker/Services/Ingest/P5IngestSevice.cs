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
using NemTracker.Model.P5Minute;
using NemTracker.Model.Stations;
using NemTracker.Persistence.Features;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class P5IngestService : IHostedService
    {
        
        private DateTime _nextRun;
        private const string Schedule = "*/5 * * * *";
        private readonly CrontabSchedule _crontabSchedule;
        
        private IConfiguration _configuration;
        
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;
        
        public P5IngestService(IConfiguration configuration)
        {
            _configuration = configuration;
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ApplicationDatabase"));
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            var nemdbContext = new NEMDBContext(optionsBuilder.Options);
            _readOnlyRepository = new ReadOnlyRepository(nemdbContext);
            _readWriteRepository = new ReadWriteRepository(nemdbContext);
            _crontabSchedule = CrontabSchedule.Parse(Schedule, 
                new CrontabSchedule.ParseOptions{IncludingSeconds = false});
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    
                    Console.WriteLine("P5 Data ingest is starting");
                    
                    var processP5DataTask = ProcessP5Data();
                    await processP5DataTask;
                    processP5DataTask.Dispose();
                    _readWriteRepository.Commit();
                    
                    Console.WriteLine("P5 Data ingest is completed");
                    
                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                    _nextRun = _nextRun.AddSeconds(90);
                    
                    await Task.Delay(UntilNextExecution(), cancellationToken);

                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task ProcessP5Data()
        {
            return Task.Run(() =>
            {
                var processor = new P5MinProcessor();
                var dataResult = processor.ProcessInstructions();

                foreach (var solutionDto in dataResult.RegionSolutionDtos)
                {
                    if (_readOnlyRepository.Table<RegionSolution, long>()
                        .Any(s => s.Interval.Equals(solutionDto.Interval) && 
                                  s.Region.Equals(solutionDto.Region)))
                    {
                        var solution = _readWriteRepository.Table<RegionSolution, long>()
                            .First(s => s.Interval.Equals(solutionDto.Interval) && 
                                        s.Region.Equals(solutionDto.Region));
                        
                        solution.Update(solutionDto);
                    }
                    else
                    {
                        var solution = RegionSolution.Create(solutionDto);
                        _readWriteRepository.Create<RegionSolution, long>(solution);
                    }
                    
                }
                
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NCrontab;
using NemTracker.Features;
using NemTracker.Model.P5Minute;
using NemTracker.Model.Stations;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class P5IngestService : IHostedService
    {
        
        private DateTime _nextRun;
        private const string Schedule = "*/5 * * * *";
        private readonly CrontabSchedule _crontabSchedule;

        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;
        
        public P5IngestService(IReadOnlyRepository readOnlyRepository,
            IReadWriteRepository readWriteRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _readWriteRepository = readWriteRepository;
            _crontabSchedule = CrontabSchedule.Parse(Schedule, 
                new CrontabSchedule.ParseOptions{IncludingSeconds = false});
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    
                    var processP5DataTask = ProcessP5Data();
                    await processP5DataTask;
                    processP5DataTask.Dispose();
                    _readWriteRepository.Commit();
                    
                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                    _nextRun = _nextRun.AddSeconds(90);
                    
                    await Task.Delay(UntilNextExecution(), cancellationToken);

                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task ProcessP5Data()
        {
            Console.WriteLine("Data ingest is starting");
            return Task.Run(() =>
            {
                var processor = new P5MinProcessor();
                var dataResult = processor.ProcessInstructions();

                foreach (var solutionDto in dataResult.RegionSolutionDtos)
                {
                    if (_readOnlyRepository.Table<RegionSolution, long>()
                        .Any(s => s.Interval.Equals(solutionDto.Interval)))
                    {
                        var solution = _readWriteRepository.Table<RegionSolution, long>()
                            .First(s => s.Interval.Equals(solutionDto.Interval));
                        
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
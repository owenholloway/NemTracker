using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NemTracker.Dtos.Reports;
using NemTracker.Features.Tools;
using NemTracker.Model.Model.Reports;
using NemTracker.Model.Observables;
using NemTracker.Persistence.Features;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Features.Ingest.Reports
{
    public class P5MinIngestObserver : IObserver<ReportDto>
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;

        private IDisposable _cancellation;

        public void OnCompleted()
        {
            //Nothing to do here
        }

        public void Subscribe(ReportHandler provider)
        {
            _cancellation = provider.Subscribe(this);
        }
        
        public void OnNext(ReportDto value)
        { 
            
            var processP5DataTask = ProcessP5Data(value);
            processP5DataTask.Wait();
            processP5DataTask.Dispose();
            _readWriteRepository.Commit();
        }
        
        public P5MinIngestObserver(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ApplicationDatabase"));
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            var nemdbContext = new NEMDBContext(optionsBuilder.Options);
            _readOnlyRepository = new ReadOnlyRepository(nemdbContext);
            _readWriteRepository = new ReadWriteRepository(nemdbContext);
        }
        
        private Task ProcessP5Data(ReportDto reportDto)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("P5 Data ingest is starting");
                
                var processor = new P5ReportProcessor();
                var regionSolutionsDtos = processor.ProcessLines(reportDto);


                foreach (var solutionDto in regionSolutionsDtos)
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

                Console.WriteLine("P5 Data ingest is completed");
                
            });
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
        
    }
}
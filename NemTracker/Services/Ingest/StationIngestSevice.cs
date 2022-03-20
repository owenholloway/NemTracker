using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NCrontab;
using NemTracker.Features;
using NemTracker.Model.Stations;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class StationIngestService : IHostedService
    {
        
        private DateTime _nextRun;
        private const string Schedule = "0 0 */10 * *";
        private readonly CrontabSchedule _crontabSchedule;

        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;
        
        public StationIngestService(IReadOnlyRepository readOnlyRepository,
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
                    var participantsTask = CompleteIngestParticipants();
                    await participantsTask;
                    participantsTask.Dispose();
                    _readWriteRepository.Commit();
                    
                    var stationsTask = CompleteIngestStations();
                    await stationsTask;
                    stationsTask.Dispose();
                    _readWriteRepository.Commit();

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                    await Task.Delay(UntilNextExecution(), cancellationToken);
                    
                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task CompleteIngestParticipants()
        {
            Console.WriteLine("Data ingest is starting");
            return Task.Run(() =>
            {
                var processor = new NemRegistrationsProcessor();
                //processor.DownloadNewXls();
                var participants = processor.GetParticipants();

                foreach (var participant in participants)
                {
                    if (
                        _readOnlyRepository.Table<Participant, Guid>()
                            .Any(P => P.Name.Equals(participant.Name)) &&
                        _readOnlyRepository.Table<Participant, Guid>()
                            .Any(P => P.Name.Equals(participant.Name))
                    ) return;
                    
                    Console.WriteLine("Adding new Station: " + participant.Name);
                    var obj = Participant.Create(participant);
                    _readWriteRepository.Create<Participant, Guid>(obj);
                }

            });
        } 
        
        private Task CompleteIngestStations()
        {
            Console.WriteLine("P5 Min Data ingest is starting");
            return Task.Run(() =>
            {
                
                Console.WriteLine("P5 Min Data ingest is complete");
                
            });
        } 
        
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
    }
}
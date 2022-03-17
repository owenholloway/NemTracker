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
using NemTracker.Model.Stations;
using NemTracker.Persistence.Features;
using Oxygen.Features;
using Oxygen.Interfaces;

namespace NemTracker.Services.Ingest
{
    public class StationIngestService : IHostedService
    {
        
        private DateTime _nextRun;
        private const string Schedule = "0 0 */10 * *";
        private readonly CrontabSchedule _crontabSchedule;
        
        private IConfiguration _configuration;
        
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;
        
        public StationIngestService(IConfiguration configuration)
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
                    Console.WriteLine("Participant Intake Start");
                    
                    var participantsTask = CompleteIngestParticipants();
                    await participantsTask;
                    participantsTask.Dispose();
                    
                    Console.WriteLine("Participant Intake Complete");
                    
                    Console.WriteLine("Station Intake Start");
                    
                    var stationsTask = CompleteIngestStations();
                    await stationsTask;
                    stationsTask.Dispose();
                    
                    Console.WriteLine("Station Intake Complete");

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                    await Task.Delay(UntilNextExecution(), cancellationToken);
                    
                }
            }, cancellationToken);
            
            return Task.CompletedTask;
            
        }

        private Task CompleteIngestParticipants()
        {
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
                    
                    var obj = Participant.Create(participant);
                    _readWriteRepository.Create<Participant, Guid>(obj);
                }

            });
        }

        private Task CompleteIngestStations()
        {
            return Task.Run(() =>
            {
                var processor = new NemRegistrationsProcessor();
                var stations = processor.GetStations();

                foreach (var station in stations)
                {
                    if (_readOnlyRepository.Table<Station, Guid>()
                        .Any(S => S.StationName.Equals(station.StationName) &&
                                  _readOnlyRepository.Table<Station, Guid>()
                                      .Any(S => S.DUID.Equals(station.DUID)))) return;

                    var participantId = Guid.Empty;
                    if (_readOnlyRepository.Table<Participant, Guid>()
                        .Any(P => P.Name.Contains(station.ParticipantName)))
                    {
                        participantId = _readOnlyRepository.Table<Participant, Guid>()
                            .First(P => P.Name.Contains(station.ParticipantName)).Id;
                    }

                    station.ParticipantId = participantId;
                    var obj = Station.Create(station);
                    _readWriteRepository.Create<Station, Guid>(obj);
                }

            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
    }
}
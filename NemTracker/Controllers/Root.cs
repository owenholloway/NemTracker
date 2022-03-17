using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NemTracker.Features;
using NemTracker.Model.Stations;
using Oxygen.Interfaces;

namespace NemTracker.Controllers
{        
    [ApiController]
    [Route("/")]
    public class RootController : Controller
    {

        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IReadWriteRepository _readWriteRepository;
        
        public RootController(IReadOnlyRepository readOnlyRepository,
            IReadWriteRepository readWriteRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _readWriteRepository = readWriteRepository;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult RootGet()
        {
            var nemProcessor = new NemRegistrationsProcessor();
            var stations = nemProcessor.GetStations();

            List<Task> tasks = new List<Task>();

            foreach (var station in stations)
            {

                Console.WriteLine("Station: " + station.StationName);
                var stationModel = Station.Create(station);
                _readWriteRepository.Create<Station, Guid>(stationModel);
                //_readWriteRepository.Commit();

            }

            return Ok("Ok");
        }
        
        
    }
}
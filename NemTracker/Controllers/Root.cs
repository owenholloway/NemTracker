using System;
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
                
            foreach (var station in stations)
            {
                Console.Write("Station: ");
                Console.Write(station.StationName);
                Console.Write(", ");
                
                Console.Write("Units Min: ");
                Console.Write(station.PhysicalUnitMin);
                Console.Write(", ");
                
                Console.Write("Units Max: ");
                Console.Write(station.PhysicalUnitMax);
                Console.WriteLine();

                var stationModel = Station.Create(station);
                _readWriteRepository.Create<Station, long>(stationModel);
                _readWriteRepository.Commit();

            }
            
            return Ok("Ok");
        }
        
        
    }
}
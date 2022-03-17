using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NemTracker.Dtos.Stations;
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
            return Ok("Ok");
        }
        
        [HttpGet]
        [Route("sites")]
        public IActionResult Sites()
        {
            var stations = _readOnlyRepository.Table<Station, Guid>().ToList();

            var stationDtos = stations.Select(station => station.GetDto()).ToList();

            return Ok(stationDtos);
        }
        
        
    }
}
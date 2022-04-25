using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using NemTracker.Dtos.Aemo;
using NemTracker.Dtos.Reports.Results;
using NemTracker.Model.Model.Reports;
using Oxygen.Interfaces;

namespace NemTracker.Controllers.Reports;

[ApiController]
[Route("/reports/p5min/")]
public class P5MinController : Controller
{
    private readonly IReadOnlyRepository _readOnlyRepository;
    private readonly IReadWriteRepository _readWriteRepository;
    private readonly MapperConfiguration _mapperConfiguration;
        
    public P5MinController(IReadOnlyRepository readOnlyRepository,
        IReadWriteRepository readWriteRepository,
            MapperConfiguration mapperConfiguration)
    {
        _readOnlyRepository = readOnlyRepository;
        _readWriteRepository = readWriteRepository;
        _mapperConfiguration = mapperConfiguration;
    }

    [HttpGet]
    [Route("rrp/{regionId}")]
    public IActionResult GetRRP(short regionId)
    {
        var rrpDtos = _readOnlyRepository
            .Table<RegionSolution, long>()
            .ProjectTo<RegionSolutionRrpDto>(_mapperConfiguration)
            .Where(rs => rs.Region == (RegionEnum)regionId);

        return Ok(rrpDtos);

    }
}
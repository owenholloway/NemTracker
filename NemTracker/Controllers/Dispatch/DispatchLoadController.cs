using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using NemTracker.Dtos.MmsData;
using NemTracker.Model.Model.MmsData.Dispatch;
using NemTracker.Persistence.Interfaces;

namespace NemTracker.Controllers.Dispatch;


[ApiController]
[Route("/reports/dispatchload/")]
public class DispatchLoadController : Controller
{

    public IMmsReadOnlyRepository _MmsReadOnlyRepository;
    private readonly MapperConfiguration _mapperConfiguration;
    
    public DispatchLoadController(
        IMmsReadOnlyRepository mmsReadOnlyRepository, 
        MapperConfiguration mapperConfiguration)
    {
        _MmsReadOnlyRepository = mmsReadOnlyRepository;
        _mapperConfiguration = mapperConfiguration;
    }
    
    
    [HttpGet]
    [Route("{duid}/tiny")]
    public IActionResult GetDispatchLoadTiny(string duid)
    {

        var result = _MmsReadOnlyRepository.Table<DispatchLoad, long>()
            .Where(dl => dl.Duid.Equals(duid))
            .ProjectTo<DispatchLoadTinyDto>(_mapperConfiguration)
            .OrderBy(dl => dl.DispatchInterval).ToList();
        
        return Ok(result);
    }
}
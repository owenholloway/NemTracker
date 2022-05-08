using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NemTracker.Model.Model.MmsData.Dispatch;
using NemTracker.Persistence.Interfaces;

namespace NemTracker.Controllers.Dispatch;


[ApiController]
[Route("/reports/dispatchload/")]
public class DispatchLoadController : Controller
{

    public IMmsReadOnlyRepository _MmsReadOnlyRepository;

    public DispatchLoadController(IMmsReadOnlyRepository mmsReadOnlyRepository)
    {
        _MmsReadOnlyRepository = mmsReadOnlyRepository;
    }
    
    
    [HttpGet]
    [Route("{duid}")]
    public IActionResult GetDuidLoad(string duid)
    {

        var result = _MmsReadOnlyRepository.Table<DispatchLoad, long>()
            .Where(dl => dl.Duid.Equals(duid)).ToList();
        
        return Ok(result);
    }
}
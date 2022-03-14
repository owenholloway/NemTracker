using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oxygen.Interfaces;

namespace NemTracker.Controllers
{        
    [ApiController]
    [Route("/")]
    public class RootController : Controller
    {

        private readonly IReadOnlyRepository _readOnlyRepository;
        
        public RootController(IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult RootGet()
        {
            return Ok("Ok");
        }
        
        
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MotivateMe.Api.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        readonly string _currentEnvironment;
        public StatusController(IConfiguration configuration)
        {
            _currentEnvironment = configuration["CurrentEnvironment"];
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok($"All good in {_currentEnvironment}");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MotivateMe.Api.Controllers
{
    [Authorize]
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly string _currentEnvironment;
        public StatusController(IConfiguration configuration)
        {
            _currentEnvironment = configuration["CurrentEnvironment"];
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok($"All good in {_currentEnvironment}");
        }

        [HttpGet("private")]
        public IActionResult GetStatusPrivate()
        {
            return Ok($"All good in private {_currentEnvironment}");
        }
    }
}
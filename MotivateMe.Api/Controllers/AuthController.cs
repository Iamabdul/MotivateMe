using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MotivateMe.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("start/{mediaIntegration}")]
        public IActionResult Start(string mediaIntegration)
        {
            if (mediaIntegration == "facebook")
                return Ok("It's facebook!");

            return BadRequest();
        }
    }
}

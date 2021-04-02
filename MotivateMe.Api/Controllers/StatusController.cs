using Microsoft.AspNetCore.Mvc;

namespace MotivateMe.Api.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok();
        }
    }
}
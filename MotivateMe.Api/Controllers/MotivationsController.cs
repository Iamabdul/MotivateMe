using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotivateMe.Api.MotivateMeContext;
using MotivateMe.Api.Services;
using MotivateMe.Core.Commands;
using MotivateMe.Core.Models;
using MotivateMe.Core.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MotivateMe.Api.Controllers
{
    [Authorize]
    [Route("api/motivations")]
    [ApiController]
    public class MotivationsController : ControllerBase
    {
        private readonly IUserResolver _userResolver;
        private readonly IMotivationQueries _motivationQueries;
        private readonly IStoreMotivationCommand _storeMotivationCommand;

        public MotivationsController(
            IUserResolver userResolver,
            IMotivationQueries motivationQueries,
            IStoreMotivationCommand storeMotivationCommand)
        {
            _userResolver = userResolver;
            _motivationQueries = motivationQueries;
            _storeMotivationCommand = storeMotivationCommand;
        }

        [HttpGet("random")]
        public async Task<IEnumerable<Motivation>> GetRandomMotivation()
        {
            await GetOrThrowForCurrentUser().ConfigureAwait(false);
            return await _motivationQueries.GetRandomMotivations().ConfigureAwait(false);
        }

        [HttpGet("type/{motivationType}")]
        public async Task<IActionResult> GetMotivationsForType(string motivationType)
        {
            var user = await GetOrThrowForCurrentUser().ConfigureAwait(false);
            if (!Enum.TryParse(typeof(MotivationType), motivationType, out var result))
                return BadRequest($"Enum type not recognised: {motivationType}");
            return Ok(await _motivationQueries.GetAllForType((MotivationType)result).ConfigureAwait(false));
        }

        [HttpGet("{motivationId}")] //motivations by user should be in the users controller?
        public async Task<IActionResult> GetMotivationsById(string motivationId)
        {
            if (!Guid.TryParse(motivationId, out _))
            {
                return BadRequest("The id of this motivation is not valid");
            }

            var user = await GetOrThrowForCurrentUser().ConfigureAwait(false);
            return Ok(await _motivationQueries.GetById(user.Id, motivationId).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<IActionResult> PostMotivationForType([FromBody] Motivation motivation)
        {
            var user = await GetOrThrowForCurrentUser().ConfigureAwait(false);
            await _storeMotivationCommand.Execute(user.Id, motivation).ConfigureAwait(false);
            return Ok();
        }

        private async Task<ApplicationUser> GetOrThrowForCurrentUser()
        {
            var user = await _userResolver.GetCurrentUser().ConfigureAwait(false);
            if (user == null)
                throw new UnauthorizedAccessException("User does not exist");
            return user;
        }
    }
}

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

        [HttpGet("type/{motivationType}")]
        public async Task<IEnumerable<Motivation>> GetMotivationsForType(string motivationType)
        {
            var user = await GetOrThrowForCurrentUser();
            if (!Enum.TryParse(typeof(MotivationType), motivationType, out var result))
                throw new ArgumentOutOfRangeException(nameof(MotivationType), $"Enum type not recognised {motivationType}");
            return await _motivationQueries.GetAllForType((MotivationType)result);
        }

        [HttpGet("{motivationId}")] //motivations by user should be in the users controller?
        public async Task<Motivation> GetMotivationsById(Guid motivationId)
        {
            var user = await GetOrThrowForCurrentUser();
            return await _motivationQueries.GetById(new Guid(user.Id), motivationId);
        }

        [HttpPost]
        public async Task<IActionResult> PostMotivationForType([FromBody] Motivation motivation)
        {
            var user = await GetOrThrowForCurrentUser();
            await _storeMotivationCommand.Execute(user.Id, motivation);
            return Ok();
        }

        private async Task<ApplicationUser> GetOrThrowForCurrentUser()
        {
            var user = await _userResolver.GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException("User does not exist");
            return user;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotivateMe.Api.MotivateMeContext;
using MotivateMe.Api.Services;
using MotivateMe.Core.Commands;
using MotivateMe.Core.Models;
using MotivateMe.Core.Queries;

namespace MotivateMe.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserResolver _userResolver;
        private readonly IMotivationQueries _motivationQueries;

        public UsersController(
            IUserResolver userResolver,
            IMotivationQueries motivationQueries)
        {
            _userResolver = userResolver;
            _motivationQueries = motivationQueries;
        }

        [HttpGet("me")]
        public async Task<ApplicationUser> GetUserInfo()
        {
            return await GetOrThrowForCurrentUser().ConfigureAwait(false);
        }

        [HttpGet("{userId}/motivations")]
        public async Task<IEnumerable<Motivation>> GetMotivationsForUser(string userId)
        {
            await GetOrThrowForCurrentUser().ConfigureAwait(false);

            return await _motivationQueries.GetAllByUserId(userId);
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

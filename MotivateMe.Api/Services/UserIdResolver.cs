using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MotivateMe.Api.Services
{
    public interface IUserIdResolver
    {
        string GetUserId();
    }

    public class UserIdResolver : IUserIdResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserIdResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
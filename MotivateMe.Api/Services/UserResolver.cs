using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MotivateMe.Api.MotivateMeContext;

namespace MotivateMe.Api.Services
{
    public interface IUserResolver
    {
        Task<ApplicationUser> GetCurrentUser();
    }

    public class UserResolver : IUserResolver
    {
        private readonly IUserIdResolver _userIdResolver;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserResolver(IUserIdResolver userIdResolver, UserManager<ApplicationUser> userManager)
        {
            _userIdResolver = userIdResolver;
            _userManager = userManager;
        }

        public Task<ApplicationUser> GetCurrentUser()
        {
            var currentUserId = _userIdResolver.GetUserId();

            if (string.IsNullOrEmpty(currentUserId))
                return Task.FromResult<ApplicationUser>(null);

            return _userManager.FindByIdAsync(currentUserId);
        }
    }
}

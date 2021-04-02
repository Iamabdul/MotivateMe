using System.Threading.Tasks;

namespace MotivateMe.Core.Commands
{
    public interface IPushMessageCommand
    {
        Task<string> Execute(string userId, string motivation);
    }

    public class PushMessageCommand : IPushMessageCommand
    {
        public Task<string> Execute(string userId, string motivation)
        {
            return Task.FromResult($"user: {userId}, your motivation is: {motivation}");
        }
    }
}
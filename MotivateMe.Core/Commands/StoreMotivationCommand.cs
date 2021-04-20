using System.Threading.Tasks;
using MotivateMe.Core.Models;
using MotivateMe.Core.Stores;

namespace MotivateMe.Core.Commands
{
    public interface IStoreMotivationCommand
    {
        Task Execute(string userId, Motivation motivation);
    }

    public class StoreMotivationCommand : IStoreMotivationCommand
    {
        private readonly IMotivationsStoreManager _motivationsStoreManager;
        public StoreMotivationCommand(IMotivationsStoreManager motivationsStoreManager)
        {
            _motivationsStoreManager = motivationsStoreManager;
        }

        public Task Execute(string userId, Motivation motivation)
        {
            return _motivationsStoreManager.InsertMotivation(userId, motivation.MotivationType, motivation.MotivationMessage);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotivateMe.Core.Models;
using MotivateMe.Core.Stores;

namespace MotivateMe.Core.Queries
{
    public interface IMotivationQueries
    {
        Task<IEnumerable<Motivation>> GetAllForType(MotivationType motivationType);
        Task<Motivation> GetById(string userId, string motivationId);
        Task<IEnumerable<Motivation>> GetAllByUserId(string userId);
        Task<IEnumerable<Motivation>> GetRandomMotivations();
    }

    public class MotivationQueries : IMotivationQueries
    {
        private readonly IMotivationsStoreManager _motivationsStoreManager;
        public MotivationQueries(IMotivationsStoreManager motivationsStoreManager)
        {
            _motivationsStoreManager = motivationsStoreManager;
        }

        public Task<IEnumerable<Motivation>> GetAllForType(MotivationType motivationType)
        {
            return _motivationsStoreManager.GetMotivationsForType(motivationType);
        }

        public Task<Motivation> GetById(string userId, string motivationId)
        {
            return _motivationsStoreManager.GetMotivationById(userId, motivationId);
        }

        public Task<IEnumerable<Motivation>> GetAllByUserId(string userId)
        {
            return _motivationsStoreManager.GetMotivationsByUserId(userId);
        }

        public Task<IEnumerable<Motivation>> GetRandomMotivations()
        {
            var enumLength = Enum.GetNames(typeof(MotivationType)).Length;
            var rand = new Random();
            return _motivationsStoreManager.GetMotivationsForType((MotivationType)rand.Next(0,enumLength));
        }
    }
}

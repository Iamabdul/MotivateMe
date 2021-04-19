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
        Task<Motivation> GetById(Guid userId, Guid motivationId);
        Task<IEnumerable<Motivation>> GetAllByUserId(Guid userId);
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

        public Task<Motivation> GetById(Guid userId, Guid motivationId)
        {
            return _motivationsStoreManager.GetMotivationById(userId, motivationId);
        }

        public Task<IEnumerable<Motivation>> GetAllByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

    }
}

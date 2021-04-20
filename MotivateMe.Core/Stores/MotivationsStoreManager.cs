using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotivateMe.Core.Models;
using MotivateMe.Core.Stores.StoreEntities;

namespace MotivateMe.Core.Stores
{
    public interface IMotivationsStoreManager
    {
        Task<IEnumerable<Motivation>> GetMotivationsForType(MotivationType motivationType);
        Task<IEnumerable<Motivation>> GetMotivationsByUserId(string userId);
        Task<Motivation> GetMotivationById(string userId, string motivationId);
        Task InsertMotivation(string userId, MotivationType motivationType, string message);
    }

    public class MotivationsStoreManager : IMotivationsStoreManager
    {
        private readonly IPartitionKeyValueStore<MotivationEntity> _motivationEntityStore;
        private readonly IPartitionKeyValueStore<MotivationByIdEntity> _motivationByIdEntityStore;

        public MotivationsStoreManager(IPartitionKeyValueStore<MotivationEntity> motivationEntityStore, 
            IPartitionKeyValueStore<MotivationByIdEntity> motivationByIdEntityStore)
        {
            _motivationEntityStore = motivationEntityStore;
            _motivationByIdEntityStore = motivationByIdEntityStore;
        }

        public async Task<IEnumerable<Motivation>> GetMotivationsForType(MotivationType motivationType)
        {
            var motivationsByType = await _motivationEntityStore.GetByPartitionAsync(motivationType.ToString());

            return motivationsByType.Select(x => new Motivation
            {
                Id = new Guid(x.RowKey),
                MotivationType = (MotivationType)Enum.Parse(typeof(MotivationType), x.PartitionKey, true),
                MotivationMessage = x.MotivationMessage
            });
        }

        public async Task<IEnumerable<Motivation>> GetMotivationsByUserId(string userId)
        {
            var motivationsByUserId = await _motivationByIdEntityStore.GetByPartitionAsync(userId);
            return motivationsByUserId.Select(x => new Motivation
            {
                Id = new Guid(x.RowKey),
                MotivationType = (MotivationType)Enum.Parse(typeof(MotivationType), x.MotivationType, true),
                MotivationMessage = x.MotivationMessage
            });
        }

        public async Task<Motivation> GetMotivationById(string userId, string motivationId)
        {
            var motivationByIdEntity = await _motivationByIdEntityStore.GetAsync(userId, motivationId);
            return new Motivation
            {
                Id = new Guid(motivationByIdEntity.RowKey),
                MotivationType = (MotivationType)Enum.Parse(typeof(MotivationType), motivationByIdEntity.MotivationType, true),
                MotivationMessage = motivationByIdEntity.MotivationMessage
            };
        }

        public Task InsertMotivation(string userId, MotivationType motivationType, string message)
        {
            var newMotivationId = Guid.NewGuid().ToString();
            var motivationEntity = new MotivationEntity(motivationType.ToString(), newMotivationId, message, userId);

            var motivationEntityTask = _motivationEntityStore.SetAsync(motivationEntity);

            var motivationByIdEntity = new MotivationByIdEntity(userId, newMotivationId, message, motivationType);

            var motivationByIdEntityTask = _motivationByIdEntityStore.SetAsync(motivationByIdEntity);

            return Task.WhenAll(new Task[]{ motivationEntityTask, motivationByIdEntityTask });
        }
    }
}

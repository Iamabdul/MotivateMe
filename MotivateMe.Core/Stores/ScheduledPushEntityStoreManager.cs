using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotivateMe.Core.Models;
using MotivateMe.Core.Stores.StoreEntities;

namespace MotivateMe.Core.Stores
{
    public interface IScheduledPushEntityStoreManager
    {
        Task<IEnumerable<ScheduledPush>> GetPushesForTimeOfDay(DateTime dateTime);
        Task<bool> InsertPushSchedule(string userId, MotivationType motivationType, DateTime timeofDay);
    }

    public class ScheduledPushEntityStoreManager : IScheduledPushEntityStoreManager
    {
        readonly IPartitionKeyValueStore<ScheduledPushEntity> _pushEntityStore;
        public ScheduledPushEntityStoreManager(IPartitionKeyValueStore<ScheduledPushEntity> pushEntityStore)
        {
            _pushEntityStore = pushEntityStore;
        }

        public async Task<IEnumerable<ScheduledPush>> GetPushesForTimeOfDay(DateTime dateTime)
        {
            var scheduledPushEntities = await _pushEntityStore.GetByPartitionAsync(dateTime.Hour.ToString());

            return scheduledPushEntities.Select(x => new ScheduledPush
            {
                UserId = new Guid(x.RowKey),
                HourOfDay = int.Parse(x.PartitionKey),
                MotivationType = (MotivationType)Enum.Parse(typeof(MotivationType), x.MotivationType, true)
            });
        }

        public async Task<bool> InsertPushSchedule(string userId, MotivationType motivationType, DateTime timeofDay)
        {
            var pushScheduleEntity = new ScheduledPushEntity(timeofDay.Hour.ToString(), userId, motivationType);

            return await _pushEntityStore.SetAsync(pushScheduleEntity);
        }
    }
}

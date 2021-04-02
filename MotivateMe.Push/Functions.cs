using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MotivateMe.Core.Models;
using MotivateMe.Core.Stores;

namespace MotivateMe.Push
{
    public class Functions
    {
        readonly IScheduledPushEntityStoreManager _scheduledPushManager;
        public Functions(IScheduledPushEntityStoreManager scheduledPushManager)
        {
            _scheduledPushManager = scheduledPushManager;
        }


        [FunctionName("Schedule_Job")]
        public async Task ScheduleJob([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            

            var pushesTodo = await _scheduledPushManager.GetPushesForTimeOfDay(DateTime.UtcNow);

            pushesTodo.ToList().ForEach(x => PretendSend(log, x.UserId, x.MotivationType));
        }

        [FunctionName("Push_Job")]
        public void PushJob([QueueTrigger("PushJob")] ILogger log)
        {
            //test
            //for (int i  = 0; i < 3; ++i)
            //{
            //    await _scheduledPushManager.InsertPushSchedule(Guid.NewGuid().ToString(), (MotivationType)i);
            //}
        }

        void PretendSend(ILogger logger, Guid userId, MotivationType motivationType)
            => logger.LogInformation($"Sending motivation to {userId} with a {motivationType} of encouragement!");
    }
}

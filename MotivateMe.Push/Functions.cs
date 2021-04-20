using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MotivateMe.Core.Commands;
using MotivateMe.Core.Models;
using MotivateMe.Core.Stores;
using MotivateMe.Core.Stores.StoreEntities;

namespace MotivateMe.Push
{
    public class Functions
    {
        private readonly IScheduledPushEntityStoreManager _scheduledPushManager;
        private readonly IPushJobQueueManager _pushJobQueueManager;
        private readonly IPushMessageCommand _pushMessageCommand;

        public Functions(
            IScheduledPushEntityStoreManager scheduledPushManager,
            IPushJobQueueManager pushJobQueueManager,
            IPushMessageCommand pushMessageCommand)
        {
            _scheduledPushManager = scheduledPushManager;
            _pushJobQueueManager = pushJobQueueManager;
            _pushMessageCommand = pushMessageCommand;
        }

        [FunctionName("Schedule_Job")]
        public async Task ScheduleJob([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger logger)
        {
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            ////test
            //for (int i = 0; i < 3; ++i)
            //{
            //    await _scheduledPushManager.InsertPushSchedule(Guid.NewGuid().ToString(), (MotivationType)i);
            //}

            var pushesTodo = await _scheduledPushManager.GetPushesForTimeOfDay(DateTime.UtcNow);

            pushesTodo.ToList().ForEach(async x => await SendToQueue(logger, x.UserId, x.MotivationType));
        }

        [FunctionName("Push_Job")]
        public async Task PushJob([QueueTrigger("pushjob")] MotivationQueueMessage queueMessage, ILogger logger)
        {
            if (!Enum.IsDefined(typeof(MotivationType), queueMessage.MotivationType))
                throw new ArgumentOutOfRangeException(nameof(MotivationType), $"Enum type not recognised {queueMessage.MotivationType}");

            Console.WriteLine(await _pushMessageCommand.Execute(queueMessage.UserId.ToString(), queueMessage.MotivationType.ToString()));
        }

        private async Task SendToQueue(ILogger logger, Guid userId, MotivationType motivationType)
        {
            await _pushJobQueueManager.PushMessage(new MotivationQueueMessage { UserId = userId, MotivationType = motivationType });
            logger.LogInformation($"Sending motivation to {userId} with a {motivationType} of encouragement!");
        }
    }
}
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using MotivateMe.Core.Stores.StoreEntities;

namespace MotivateMe.Core.Stores
{
    public interface IPushJobQueueManager
    {
        Task PushMessage(MotivationQueueMessage motivationQueueMessage);
    }
    public class PushJobQueueManager : IPushJobQueueManager
    {
        readonly QueueClient _cloudQueue;
        readonly string _queueName = "pushjob";

        public PushJobQueueManager(IConfiguration configuration)
        {
            _cloudQueue = new QueueClient(configuration["AzureWebJobsStorage"], _queueName);
        }

        public async Task PushMessage(MotivationQueueMessage motivationQueueMessage)
        {
            var serialised = Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(motivationQueueMessage));
            await _cloudQueue.SendMessageAsync(serialised);
        }
    }
}

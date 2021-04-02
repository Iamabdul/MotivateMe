using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotivateMe.Core.Commands;
using MotivateMe.Core.Stores;
using MotivateMe.Core.Stores.StoreEntities;

namespace MotivateMe.Core.DIConfiguration
{
    public static class CoreDiConfiguration
    {
        public static IServiceCollection ConfigureFormCore(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterStoresAndManagers(services);
            RegisterCommands(services);

            services.AddSingleton(CloudStorageAccount.Parse(configuration["AzureWebJobsStorage"]));
            return services;
        }

        private static void RegisterStoresAndManagers(IServiceCollection services)
        {
            //Stores
            services.AddSingleton<IPartitionKeyValueStore<MotivationEntity>, AzureStorageTableEntityStore<MotivationEntity>>();
            services.AddSingleton<IPartitionKeyValueStore<ScheduledPushEntity>, AzureStorageTableEntityStore<ScheduledPushEntity>>();


            //Managers
            services.AddSingleton<IScheduledPushEntityStoreManager, ScheduledPushEntityStoreManager>();
            services.AddSingleton<IPushJobQueueManager, PushJobQueueManager>();
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            services.AddSingleton<IPushMessageCommand, PushMessageCommand>();
            services.AddSingleton<IStoreMotivationCommand, StoreMotivationCommand>();
        }
    }
}
using Microsoft.Azure.Cosmos.Table;
using MotivateMe.Core.Models;

namespace MotivateMe.Core.Stores.StoreEntities
{
    public class ScheduledPushEntity : TableEntity
    {
        public ScheduledPushEntity()
        {
        }

        public ScheduledPushEntity(string partitionKey, string rowKey, MotivationType motivationType)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            MotivationType = motivationType.ToString();
        }

        public string MotivationType { get; set; }
    }
}
using Microsoft.Azure.Cosmos.Table;

namespace MotivateMe.Core.Stores.StoreEntities
{
    public class MotivationEntity : TableEntity
    {
        public MotivationEntity()
        {
        }

        public MotivationEntity(string motivationType, string motivationId, string motivationMessage, string userId)
        {
            PartitionKey = motivationType;
            RowKey = motivationId;
            MotivationMessage = motivationMessage;
            UserId = userId;
        }

        public string MotivationMessage { get; set; }
        public string UserId { get; set; }
    }
}
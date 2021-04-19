using Microsoft.Azure.Cosmos.Table;
using MotivateMe.Core.Models;

namespace MotivateMe.Core.Stores.StoreEntities
{
    public class MotivationByIdEntity : TableEntity
    {
        public MotivationByIdEntity()
        {

        }

        public MotivationByIdEntity(string userId, string motivationId, string motivationMessage, MotivationType motivationType)
        {
            PartitionKey = userId;
            RowKey = motivationId;
            MotivationMessage = motivationMessage;
            UserId = userId;
            MotivationType = motivationType.ToString();
        }

        public string MotivationMessage { get; set; }
        public string UserId { get; set; }
        public string MotivationType { get; set; }
    }
}
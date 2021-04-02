using Microsoft.Azure.Cosmos.Table;

namespace MotivateMe.Core.Stores.StoreEntities
{
    public class StandardTableEntity : TableEntity
    {

        public StandardTableEntity()
        {

        }

        public StandardTableEntity(string partitionKey, string rowKey, string value)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Value = value;
        }

        public string Value { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using MotivateMe.Core.Extensions;

namespace MotivateMe.Core.Stores
{
    public class AzureStorageTableEntityStore<T> : IPartitionKeyValueStore<T> where T : ITableEntity, new ()
    {
        static readonly IRetryPolicy RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(0.2), 10);

        readonly CloudTable _cloudTable;
        readonly OperationContext OperationContext = new OperationContext();
        readonly TableRequestOptions TableRequestOptions = new TableRequestOptions
        {
            RetryPolicy = RetryPolicy
        };

        public AzureStorageTableEntityStore(CloudStorageAccount storageAccount)
        {
            var tableClient =  storageAccount.CreateCloudTableClient();
            tableClient.InitTable<T>().GetAwaiter().GetResult();
            _cloudTable = tableClient.GetTable<T>();
        }

        public async Task<T> GetAsync(string partition, string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partition, rowKey);
            var result = await _cloudTable.ExecuteAsync(retrieveOperation);
            return result.GetTheT<T>();
        }

        public async Task<IEnumerable<T>> GetByPartitionAsync(string partitionKey)
        {
            var items = new List<T>();
            var continuationToken = (TableContinuationToken)null;
            var cancellationToken = (CancellationToken)default;
            do
            {
                var seg = await _cloudTable.ExecuteQuerySegmentedAsync(QueryByPartitionKey(partitionKey), continuationToken, TableRequestOptions, OperationContext, cancellationToken);

                items.AddRange(seg.Results.Select(entity => entity));

                continuationToken = seg.ContinuationToken;

            } while (continuationToken != null && cancellationToken.IsCancellationRequested == false);

            return items;
        }

        public async Task<bool> SetAsync(T value)
        {
            var insertOpration = TableOperation.InsertOrReplace(value);

            var result = await _cloudTable.ExecuteAsync(insertOpration);

            return result.HttpStatusCode < 400;
        }

        public Task<bool> DeleteAsync(string partition, string rowKey)
        {
            throw new NotImplementedException();
        }

        private TableQuery<T> QueryByPartitionKey(string partitionKey) => new TableQuery<T>()
            .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotivateMe.Core.Stores
{
    public interface IPartitionKeyValueStore<T>
    {
        Task<T> GetAsync(string partition, string rowKey);
        Task<bool> SetAsync(T value);
        Task<bool> DeleteAsync(string partition, string rowKey);
        Task<IEnumerable<T>> GetByPartitionAsync(string partitionKey);
    }
}
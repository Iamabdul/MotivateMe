using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace MotivateMe.Core.Extensions
{
    public static class CloudTableExtensions
    {
        public static CloudTable GetTable<T>(this CloudTableClient client)
        {
            return client.GetTableReference(typeof(T).Name.Replace("Entity", "Table").ToLower());
        }

        public async static Task InitTable<T>(this CloudTableClient client)
        {
            await client.GetTable<T>().CreateIfNotExistsAsync();
        }

        public static T GetTheT<T>(this TableResult tableResult)
        {
            if (tableResult?.Result == null)
            {
                return default;
            }

            return (T)tableResult.Result;
        }
    }
}
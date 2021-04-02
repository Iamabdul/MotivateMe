using Microsoft.Azure.Cosmos.Table;
using MotivateMe.Core.Models;

namespace MotivateMe.Core.Stores.StoreEntities
{
    public class MotivationEntity : TableEntity
    {
        public MotivationType MotivationType { get; set; }
        public string MotivationMessage { get; set; }
    }
}
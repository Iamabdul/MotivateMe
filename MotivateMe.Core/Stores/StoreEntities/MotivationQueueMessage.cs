using System;
using MotivateMe.Core.Models;

namespace MotivateMe.Core.Stores.StoreEntities
{
    public class MotivationQueueMessage
    {
        public Guid UserId { get; set; }
        public MotivationType MotivationType { get; set; }
    }
}
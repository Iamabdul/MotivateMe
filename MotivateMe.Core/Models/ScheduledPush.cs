using System;

namespace MotivateMe.Core.Models
{
    public class ScheduledPush
    {
        public int HourOfDay { get; set; }
        public Guid UserId { get; set; }
        public MotivationType MotivationType { get; set; }
    }
}

using System;

namespace MotivateMe.Core.Models
{
    public class Motivation
    {
        public Guid Id { get; set; }
        public MotivationType MotivationType { get; set; }
        public string MotivationMessage { get; set; }
    }    
}

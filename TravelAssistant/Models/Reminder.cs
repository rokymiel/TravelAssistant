using System;
namespace TravelAssistant.Models
{
    public class Reminder:Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Position { get; set; }
        public int Priority { get; set; }
    }
}

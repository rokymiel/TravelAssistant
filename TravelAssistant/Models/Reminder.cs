using System;
namespace TravelAssistant.Models
{
    public class Reminder : Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Position { get; set; }
        public int Priority { get; set; }
        public string PriorityIcon { get => Priority > 0 ? $"priorityIcon{Priority}.png" : null; }
        public bool HasNotification { get; set; } = false;
        public int NotificationId { get; set; }
        public bool NotNotified { get => Date > DateTime.Now; }
        //public string IsAddType { get => Type == OperationType.Add ? "Green" : "Black"; }
        public string DateColor { get => NotNotified ? "Gray" : "Red"; }
        public string StringDate
        {
            get
            {
                if (Date == null || Date.Ticks == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return $"{Date.ToString("g")}";
                }

            }
        }
    }
}

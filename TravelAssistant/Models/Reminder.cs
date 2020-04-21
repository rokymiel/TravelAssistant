using System;
namespace TravelAssistant.Models
{
    public class Reminder : TripData
    {
        /// <summary>
        /// Название напоминания.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание/заметки напоминания.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Дата уведомления.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Местоположение напоминания.
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// Приоритет.
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Иконка приоритета.
        /// </summary>
        public string PriorityIcon { get => Priority > 0 ? $"priorityIcon{Priority}.png" : null; }
        /// <summary>
        /// Есть ли уведомление.
        /// </summary>
        public bool HasNotification { get; set; } = false;
        /// <summary>
        /// Id уведомления.
        /// </summary>
        public int NotificationId { get; set; }
        /// <summary>
        /// Не просрочено ли уведомление.
        /// </summary>
        public bool NotNotified { get => Date > DateTime.Now; }
        /// <summary>
        /// Цвет даты уведомления.
        /// </summary>
        public string DateColor { get => NotNotified ? "Gray" : "Red"; }
        /// <summary>
        /// Формат даты.
        /// </summary>
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

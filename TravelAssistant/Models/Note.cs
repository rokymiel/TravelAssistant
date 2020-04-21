using System;
using SQLite;
namespace TravelAssistant.Models
{
    
    public class Note:TripData
    {
        /// <summary>
        /// Название заметки.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Текст заметки.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Дата последнего редактироания.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Форматированная дата заметки.
        /// </summary>
        public string FormatedDate { get => $"{Date.Day},{Date.ToString("ddd")}"; } 

        
    }
}

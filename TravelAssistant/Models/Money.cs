using System;
namespace TravelAssistant.Models
{
    public class Money:TripData
    {
        /// <summary>
        /// Текущие деньги.
        /// </summary>
        public int CurrentMoney { get; set; }
        /// <summary>
        /// Все деньги.
        /// </summary>
        public int AllMoney { get; set; }
    }
}

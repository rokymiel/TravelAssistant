using System;
using System.Globalization;

namespace TravelAssistant.Models
{
    /// <summary>
    /// Финансовая операция.
    /// </summary>
    public class MoneyOperation : TripData
    {
        /// <summary>
        /// Описание операции.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Тип операции.
        /// </summary>
        public OperationType Type { get; set; }
        /// <summary>
        /// Дата добавления операции.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Цвет текста операции.
        /// </summary>
        public string IsAddType { get => Type == OperationType.Add ? "Green" : "Black"; }
        /// <summary>
        /// Название операции.
        /// </summary>
        public string GetOperationType { get => Type == OperationType.Add ? "Пополнение" : Type == OperationType.Minus ? Description : null; }
        /// <summary>
        /// Численная информация об операции.
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// Форматированная дата операции.
        /// </summary>
        public string DateString {get => $"{Date.ToString("d MMMM yyyy, H:mm")}";}
        /// <summary>
        /// Форматированная численная информация об операции.
        /// </summary>
        public string MoneyString
        {
            get
            {
                if (Type == OperationType.Add)
                    return $"+{string.Format($"{Money:N}")}";
                else
                    return $"-{string.Format($"{Money:N}")}";

            }
        }

        public enum OperationType
        {
            Add,
            Minus
        }

    }
}

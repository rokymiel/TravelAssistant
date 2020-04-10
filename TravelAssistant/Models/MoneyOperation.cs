using System;
using System.Globalization;

namespace TravelAssistant.Models
{
    public class MoneyOperation : Item
    {
        public string Description { get; set; }
        public OperationType Type { get; set; }
        public DateTime Date { get; set; }
        public string IsAddType { get => Type == OperationType.Add ? "Green" : "Black"; }
        public string GetOperationType { get => Type == OperationType.Add ? "Пополнение" : Type == OperationType.Minus ? Description : null; }
        public int Money { get; set; }
        public string DateString {get => $"{Date.ToString("d MMMM yyyy, H:mm")}";}
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

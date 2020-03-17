using System;
namespace TravelAssistant.Models
{
    public class MoneyOperation : Item
    {
        public string Description { get; set; }
        public OperationType Type { get; set; }
        public string GetType { get => Type == OperationType.Add ? "Пополнение" : Type == OperationType.Minus ? "Покупка" : null; }
        public int Money { get; set; }
        public enum OperationType
        {
            Add,
            Minus
        }
        
    }
}

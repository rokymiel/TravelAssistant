using System;
using SQLite;
namespace TravelAssistant.Models
{
    
    public class Note:Item
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string FormatedDate { get => $"{Date.Day},{Date.ToString("ddd")}"; } 

        
    }
}

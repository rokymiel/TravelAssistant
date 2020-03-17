using System;
using SQLite;
namespace TravelAssistant.Models
{
    public class Item
    {
        [PrimaryKey]
        public string Id { get; set; }
        public Item() { }

    }
}

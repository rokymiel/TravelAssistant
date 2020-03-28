using System;
using SQLite;
namespace TravelAssistant.Models
{
    public class Item
    {
        [PrimaryKey]
        public virtual string Id { get; set; }
        public Item() { }

    }
}

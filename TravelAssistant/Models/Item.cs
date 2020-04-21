using System;
using SQLite;
namespace TravelAssistant.Models
{
    public class Item
    {
        /// <summary>
        /// Id объекта для хранения в БД.
        /// </summary>
        [PrimaryKey]
        public virtual string Id { get; set; }
        public Item() { }

    }
}

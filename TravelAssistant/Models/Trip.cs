using System;
using System.Collections.Generic;

namespace TravelAssistant.Models
{
    public class Trip : Item
    {
        public Trip(string country, string city, DateTime beginingDate, DateTime endingDate)
        {
            Country = country;
            City = city;
            BeginingDate = beginingDate;
            EndingDate = endingDate;
        }
        public Trip() { }
        /// <summary>
        /// Дата поездки в виде строки.
        /// </summary>
        public string Date { get => $"{BeginingDate.ToString("dd.MM.yyyy")} - {EndingDate.ToString("dd.MM.yyyy")}"; }
        /// <summary>
        /// Страна поездки.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Город поездки.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Дата начала поездки.
        /// </summary>
        public DateTime BeginingDate { get; set; }
        /// <summary>
        /// Дата окончания поездки.
        /// </summary>
        public DateTime EndingDate { get; set; }
        /// <summary>
        /// Название поездки.
        /// </summary>
        public string Name { get; set; }
        
    }

}

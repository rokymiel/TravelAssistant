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
        public string Date { get => $"{BeginingDate.ToString("dd.MM.yyyy")} - {EndingDate.ToString("dd.MM.yyyy")}"; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime BeginingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Name { get; set; }
        
    }
    public class Nad
    {
        public string Name { get; set; }
    }

}

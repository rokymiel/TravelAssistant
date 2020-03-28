using System;
using SQLite;
namespace TravelAssistant.Models
{
    public class Place:Item
    {
        public Place(Venue venue)
        {
            Address = venue.location.address;
            CrossStreet = venue.location.crossStreet;
            Lat = venue.location.lat;
            Lng = venue.location.lng;
            Id = venue.id;
            Name = venue.name;
        }
        public Place()
        {

        }
        public string Address { get; set; }
        public string CrossStreet { get; set; }
        public double Lat { get;  set; }
        public double Lng { get; set; }
        public string Name { get; set; }
        [PrimaryKey]
        public override string Id { get;  set; }

    }
}

using System;
namespace TravelAssistant.Models
{
    public class Place
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
        public string Address { get; private set; }
        public string CrossStreet { get; private set; }
        public double Lat { get; private set; }
        public double Lng { get; private set; }
        public string Id { get; private set; }
        public string Name { get; private set; }
    }
}

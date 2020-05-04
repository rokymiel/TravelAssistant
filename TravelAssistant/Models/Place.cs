using System;
using SQLite;
namespace TravelAssistant.Models
{
    public class Place:TripData
    {
        public Place(Venue venue,string tripId)
        {
            Address = venue.location.address;
            CrossStreet = venue.location.crossStreet;
            Lat = venue.location.lat;
            Lng = venue.location.lng;
            PlaceId = venue.id;
            TripId = tripId;
            Id = PlaceId + TripId;
            Name = venue.name;
        }
        public Place()
        {

        }
        /// <summary>
        /// Адресс.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Пересекающая улица.
        /// </summary>
        public string CrossStreet { get; set; }
        /// <summary>
        /// Координата ширины.
        /// </summary>
        public double Lat { get;  set; }
        /// <summary>
        /// Координата долготы.
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id места.
        /// </summary>
        public string PlaceId { get; set; }

    }
}

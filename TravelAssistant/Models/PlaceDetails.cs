using System;
namespace TravelAssistant.Models
{
    public class PlaceDatails
    {
        public Meta meta;
        public PlaceResponse response;
        public override string ToString()
        {
            return response.venue.ToString();
        }
    }
    public class PlaceResponse
    {
        public VenueDetails venue;
    }
    public class VenueDetails
    {
        public string id;
        public string name;
        public Contact contact;
        public string url;
        public string description;
        public double rating;
        public BestPhoto bestPhoto;
        public override string ToString()
        {
            return $"id:{id}{Environment.NewLine}" +
                $"name:{name}{Environment.NewLine}" +
                $"url:{url}{Environment.NewLine}" +
                $"descr:{description}{Environment.NewLine}" +
                $"rating:{rating}{Environment.NewLine}" +
                $"photo:{bestPhoto}{Environment.NewLine}"; ;
        }

    }
    public class BestPhoto
    {
        public string id;
        public string prefix;
        public string suffix;
        public override string ToString()
        {
            return $"{prefix} {suffix}";
        }
    }
    public class Contact
    {
        public string phone;
        public string formattedPhone;
        public string twitter;
        public string instagram;
        public string facebook;
        public string facebookUsername;
        public string facebookName;
    }
}

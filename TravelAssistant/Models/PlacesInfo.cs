using System;
namespace TravelAssistant.Models
{
    public class PlacesInfo
    {
        public Meta meta;
        public Response response;
        public override string ToString()
        {
            string res = "";
            res = $"Meta: code:{meta.code}{Environment.NewLine}" +
                $"Response: {response.GroupsToDtring()}";
            return res;
        }
    }
    public class Meta
    {
        public int code;
    }
    public class Response
    {
        public Items[] groups;
        public string GroupsToDtring()
        {
            string res = "";
            foreach (var item in groups)
            {
                res += item.ToString() + Environment.NewLine;
            }
            return res;
        }
    }
    public class Items
    {
        public ItemVenue[] items;
        public override string ToString()
        {
            string s = "";
            foreach (var item in items)
            {
                s += $"\tItems{Environment.NewLine}{item}{Environment.NewLine}";
            }
            return s;
        }
    }
    public class ItemVenue
    {
        public override string ToString()
        {
            return venue.ToString();
        }
        public Venue venue;
    }
    public class Venue
    {
        public override string ToString()
        {
            string s = $"id:{id}{Environment.NewLine}" +
                $"name:{name}{Environment.NewLine}" +
                $"Location:{location.address}\t{location.crossStreet}\t{location.lat}\t{location.lng}{Environment.NewLine}" +
                $"Catigories:{CategoriesToString()}";
            return s;
        }
        private string CategoriesToString()
        {
            string res = "";
            foreach (var item in categories)
            {
                res += $"{item.id}\t{item.name}" + Environment.NewLine;
            }
            return res;
        }
        public string id;
        public string name;
        public PlaceLocation location;
        public Categories[] categories;
    }
    public class PlaceLocation
    {
        public string address;
        public string crossStreet;
        public double lat;
        public double lng;
    }
    public class Categories
    {
        public string id;
        public string name { get; set; }
    }
}

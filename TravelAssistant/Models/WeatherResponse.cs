using System;
namespace TravelAssistant.Models
{
    public class WeatherResponse
    {
        public WeatherSituationRespose[] weather;
        public TemperatureResponse main;
        public string name;
        public string GetWeather
        {
            get
            {
                string s = "";
                foreach (var item in weather)
                {
                    s += item.ToString();
                }
                return s;
            }
        }
        public override string ToString()
        {
            return $"Weather{Environment.NewLine}{GetWeather}{Environment.NewLine}Main{Environment.NewLine}{main}";
        }

    }
    public class TemperatureResponse
    {
        public double temp;
        public double temp_min;
        public double temp_max;
        public double feels_like;
        public override string ToString()
        {
            return $"\ttemp={temp}{Environment.NewLine}" +
                $"\t mintemp={temp_min}{Environment.NewLine}" +
                $"\t maxtemp={temp_max}{Environment.NewLine}" +
                $"\t feels like={feels_like}";
        }
    }
    public class WeatherSituationRespose
    {
        public int id;
        public string main;
        public string description;
        public string icon;
        public override string ToString()
        {
            return $"\tid={id}{Environment.NewLine}" +
                $"\tmain={main}{Environment.NewLine}" +
                $"\tdescription={description}{Environment.NewLine}";
        }
    }
}

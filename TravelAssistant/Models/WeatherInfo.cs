﻿using System;
namespace TravelAssistant.Models
{
    public class WeatherInfo
    {
        public WeatherInfo(WeatherResponse weather)
        {
            City = weather.name;
            CurrentTemperature = weather.main.temp;
            MinimumTemperature = weather.main.temp_min;
            MaximumTemperature = weather.main.temp_max;
            FeelsLikeTemperature = weather.main.feels_like;
            Condition = WeatherCondition.GetCondisionsArray(weather.weather);

        }
        public string City { get; private set; }
        public double CurrentTemperature { get; private set; }
        public double MinimumTemperature { get; private set; }
        public double MaximumTemperature { get; private set; }
        public double FeelsLikeTemperature { get; private set; }
        public WeatherCondition[] Condition { get; set; }
        public class WeatherCondition
        {
            public WeatherCondition(WeatherSituationRespose response)
            {
                Id = response.id;
                Condition = response.main;
                Description = response.description;
            }
            public int Id { get; set; }
            public string Condition { get; set; }
            /// <summary>
            /// In local language.
            /// </summary>
            public string Description { get; set; }
            public static WeatherCondition[] GetCondisionsArray(WeatherSituationRespose[] respose)
            {
                WeatherCondition[] conditions = new WeatherCondition[respose.Length];
                int i = 0;
                foreach (var item in respose)
                {
                    conditions[i] = new WeatherCondition(item);
                    ++i;
                }
                return conditions;
            }
        }

    }
}
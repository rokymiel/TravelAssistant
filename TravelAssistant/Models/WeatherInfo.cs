using System;
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
        /// <summary>
        /// Дата последнего запроса.
        /// </summary>
        public DateTime DateOfRequest { get; set; }
        /// <summary>
        /// Город.
        /// </summary>
        public string City { get; private set; }
        /// <summary>
        /// Текущая температура.
        /// </summary>
        public double CurrentTemperature { get; private set; }
        /// <summary>
        /// Минимальная температура.
        /// </summary>
        public double MinimumTemperature { get; private set; }
        /// <summary>
        /// Максимальная температура.
        /// </summary>
        public double MaximumTemperature { get; private set; }
        /// <summary>
        /// Ощущение температуры.
        /// </summary>
        public double FeelsLikeTemperature { get; private set; }
        /// <summary>
        /// Состояние температуры.
        /// </summary>
        public WeatherCondition[] Condition { get; set; }
        public class WeatherCondition
        {
            public WeatherCondition(WeatherSituationRespose response)
            {
                Id = response.id;
                Condition = response.main;
                Description = response.description;
                Icon = GetFormatedIconPath(response.icon);

            }
            /// <summary>
            /// Форматирование названия иконки.
            /// </summary>
            /// <param name="icon">Название.</param>
            /// <returns>Форматированное название.</returns>
            public string GetFormatedIconPath(string icon) => icon.Substring(2) + icon.Substring(0, 2);

            /// <summary>
            /// Id состояния.
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// Состояние.
            /// </summary>
            public string Condition { get; set; }
            /// <summary>
            /// Описание состояния.
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// Id иконки погоды.
            /// </summary>
            public string Icon { get; set; }
            /// <summary>
            /// Путь иконки.
            /// </summary>
            public string IconPath { get => $"{Icon}.png"; }
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

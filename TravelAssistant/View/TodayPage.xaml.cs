using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TravelAssistant.Models;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.PancakeView;
using TravelAssistant.Managers;
using TravelAssistant.Custom;

namespace TravelAssistant.View
{
    public partial class TodayPage : ContentPage
    {
        /// <summary>
        /// Базовая ссылка для рекомендаций.
        /// </summary>
        const string recUrl = "https://api.foursquare.com/v2/venues/explore?client_id=GMDFIDPK1TFXA45NFQVXDI15K2HEV5OHOHDRTAY2HG1XMUSQ&client_secret=NLRNJD0BN32HSXPIRQRMXDU5FYGYV4CVQ1LRWUV15UGOORDR&v=20190425";
        /// <summary>
        /// Префикс запроса погоды.
        /// </summary>
        const string weatherUrlPrefix = "https://ru.api.openweathermap.org/data/2.5/weather?";
        /// <summary>
        /// Постфикс запроса погоды.
        /// </summary>
        const string weatherUrlSafix = "&units=metric&lang=ru&appid=339905ee10b8292e3cf6d11569aacf96";
        /// <summary>
        /// Бюджет.
        /// </summary>
        public Money money;
        /// <summary>
        /// Погода уже обновлена.
        /// </summary>
        private bool weatherUpdate = false;
        /// <summary>
        /// Первый запрос местоположения.
        /// </summary>
        private bool isFirstGetLocation = true;
        /// <summary>
        /// Список мест.
        /// </summary>
        ObservableCollection<Place> places;
        /// <summary>
        /// Местоположение пользователя.
        /// </summary>
        Xamarin.Essentials.Location location;
        /// <summary>
        /// Информация о погоде.
        /// </summary>
        WeatherInfo weatherInfo;

        public TodayPage()
        {
            InitializeComponent();
            places = new ObservableCollection<Place>();
            RecomendationCards.ItemsSource = places;
        }
        /// <summary>
        /// Получение текущей даты.
        /// </summary>
        /// <returns>Текущая дата.</returns>
        private string GetCurrentDate()
        {
            var date = DateTime.Now;
            return date.Day + " " + date.ToString("dddd");
        }

        protected override void OnAppearing()
        {
            date.Text = GetCurrentDate();
            var m = App.moneyManager.GetTripItems<Money>(MainPage.CurrentTrip.Id);
            if (m != null && m.Count > 0)
                money = m[0];
            else
                money = null;
            if (money != null)
            {
                allMoneyLabel.Text = money.AllMoney.ToString();
                currentMoneyLabel.Text = money.CurrentMoney.ToString();
                if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);
            }
            else
            {
                allMoneyLabel.Text = "0";
                currentMoneyLabel.Text = "0";
                moneyBar.Progress = 0;
            }
            if (isFirstGetLocation)
            {
                indicator.IsRunning = true;
                Func<Task> func = DoWeather;
                func += DoReq;
                GetLocation(func);
                isFirstGetLocation = false;
            }
        }
        /// <summary>
        /// Отрисовка информации о погоде.
        /// </summary>
        private void WeatherUpdate()
        {

            currentTempLabel.Text = GetStringTemperarure((int)weatherInfo.CurrentTemperature);

            minTemLabel.Text = GetStringTemperarure((int)weatherInfo.MinimumTemperature);
            maxTemLabel.Text = GetStringTemperarure((int)weatherInfo.MaximumTemperature);
            dateOfRequest.Text = weatherInfo.DateOfRequest.ToString("H:mm");
            if (weatherInfo.Condition != null && weatherInfo.Condition.Length > 0)
            {
                weatherDescrLabel.Text = UpFirstChar(weatherInfo.Condition[0].Description);
                weatherIcon.Source = weatherInfo.Condition[0].IconPath;
            }
            weatherCityLabel.Text = weatherInfo.City;
            feelsLabel.Text = $"Ощушается как {GetStringTemperarure((int)weatherInfo.FeelsLikeTemperature)}";
        }
        /// <summary>
        /// Получение температуры из числа.
        /// </summary>
        public string GetStringTemperarure(int temp)
        {
            return temp > 0 ? "+" + temp + "°" : temp + "°";
        }
        /// <summary>
        /// Подъем первого символа.
        /// </summary>
        public string UpFirstChar(string s)
        {
            var safix = s.Substring(1);

            return char.ToUpper(s[0]) + safix;
        }
        /// <summary>
        /// Обновление погоды.
        /// </summary>
        void WeatherUpdate_Clicked(System.Object sender, System.EventArgs e)
        {
            weatherUpdate = true;
            GetLocation(DoWeather);
        }

        /// <summary>
        /// Работа с погодой.
        /// </summary>
        /// <returns></returns>
        async private Task DoWeather()
        {
            if (location != null)
            {

                if (App.Current.Properties.ContainsKey("weatherJSON") && App.Current.Properties.ContainsKey("weatherDate"))
                {
                    var dor = (DateTime)App.Current.Properties["weatherDate"];
                    var res = DateTime.Now - dor;
                    if (res.Ticks < 2 * TimeSpan.TicksPerMinute)
                    {
                        if (weatherUpdate)
                        {
                            weatherUpdate = false;
                            return;
                        }
                        weatherInfo = new WeatherInfo(JsonConvert.DeserializeObject<WeatherResponse>((string)App.Current.Properties["weatherJSON"]));
                        weatherInfo.DateOfRequest = dor;
                        WeatherUpdate();
                        return;
                    }
                }
                try
                {
                    DateTime time = DateTime.Now;
                    WebRequest request = WebRequest.Create(weatherUrlPrefix + $"lat={GetDouble(location.Latitude)}&lon={GetDouble(location.Longitude)}" + weatherUrlSafix);
                    WebResponse responseW = await request.GetResponseAsync();

                    string response;
                    using (StreamReader streamReader = new StreamReader(responseW.GetResponseStream()))
                    {
                        response = streamReader.ReadToEnd();
                    }
                    WeatherResponse weather = JsonConvert.DeserializeObject<WeatherResponse>(response);
                    weatherInfo = new WeatherInfo(weather) { DateOfRequest = DateTime.Now };
                    App.Current.Properties["weatherJSON"] = response;
                    App.Current.Properties["weatherDate"] = DateTime.Now;

                    Console.WriteLine(weatherInfo);
                    WeatherUpdate();
                }
                catch (WebException ex)
                {
                    await DisplayAlert("Ошибка", "Проблемы с интернетом", "OK");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await DisplayAlert("Ошибка", "Произошла непредвиденная ошибка", "OK");
                }
            }
        }
        /// <summary>
        /// Получение рекомендации.
        /// </summary>
        async private Task DoReq()
        {
            if (location != null)
            {
                string safix = $"&ll={GetDouble(location.Latitude)},{GetDouble(location.Longitude)}&categoryId=4deefb944765f83613cdba6e,4bf58dd8d48988d181941735,5642206c498e4bfca532186c&limit=15&radius=20000&locale=ru";

                try
                {
                    PlacesInfo placesInfo = await GetPlacesRecomendation(recUrl + safix);
                    if (placesInfo.meta.code == 200)
                    {
                        indicator.IsRunning = false;
                        MakePlacesList(placesInfo);
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", $"Код: {placesInfo.meta.code}", "OK");
                    }
                }
                catch (WebException ex)
                {
                    await DisplayAlert("Ошибка", "Проблемы с интернетом", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Ошибка", "Произошла непредвиденная ошибка", "OK");
                }
                finally
                {
                    indicator.IsRunning = false;
                }

            }
        }
        /// <summary>
        /// Получение списка мест.
        /// </summary>
        /// <param name="placesInfo"></param>
        private void MakePlacesList(PlacesInfo placesInfo)
        {
            foreach (var groups in placesInfo.response.groups)
            {
                foreach (var items in groups.items)
                {
                    Place newPlace = new Place(items.venue, MainPage.CurrentTrip.Id);
                    places.Add(newPlace);
                }
            }
        }
        /// <summary>
        /// Получение числа с точкой в нужном формате.
        /// </summary>
        private string GetDouble(double d)
        {
            string req = d.ToString();
            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                req = req.Replace(',', '.');
            }
            return req;
        }

        /// <summary>
        /// Получение местоположение и выполнение какого-то действия.
        /// </summary>
        private async void GetLocation(Func<Task> action)
        {
            try
            {
                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium);

                location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    await action();
                }
                else
                {
                    indicator.IsRunning = false;
                }
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Ошибка", "Не удается определить местоположение", "OK");
            }
            catch (Xamarin.Essentials.FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await DisplayAlert("Ошибка", "Не удается определить местоположение", "OK");
            }
            catch (Xamarin.Essentials.PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Ошибка", "Не удается определить местоположение", "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location

                await DisplayAlert("Ошибка", "Не удается определить местоположение " + ex.Message, "OK");


            }
        }

        /// <summary>
        /// Запрос на получение списка рекомендаций.
        /// </summary>
        async private Task<PlacesInfo> GetPlacesRecomendation(string url)
        {

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            PlacesInfo placesInfo = JsonConvert.DeserializeObject<PlacesInfo>(response);
            return placesInfo;
        }
        /// <summary>
        /// Открытие выбранного места.
        /// </summary>
        async void OnPlaceSelected(Object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Place;
            if (item == null)
            {

                RecomendationCards.SelectedItem = null;
                return;
            }
            await Navigation.PushAsync(new PlaceDatailsPage(item));
            //await Navigation.PushAsync(new PlaceInfoPage(item));
            RecomendationCards.SelectedItem = null;
        }
        /// <summary>
        /// Открытие финансов.
        /// </summary>
        async void FinanceWidget_Tapped(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            await Navigation.PushAsync(new FinancePage());
        }
        /// <summary>
        /// Открытие сохраненных мест.
        /// </summary>
        async void RecomedationSaved_Clicked(System.Object sender, System.EventArgs e)
        {
            await recomedationSavedButton.FadeTo(0.4, 100);
            //await Task.Delay(50);
            await recomedationSavedButton.FadeTo(1, 100);
            await Navigation.PushModalAsync(new NavigationCustomPage(new RecomedationSavedPage()));
        }


    }
}

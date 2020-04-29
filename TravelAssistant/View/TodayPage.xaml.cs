﻿using System;
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

namespace TravelAssistant.View
{
    public partial class TodayPage : ContentPage
    {
        string recUrl = "https://api.foursquare.com/v2/venues/explore?client_id=GMDFIDPK1TFXA45NFQVXDI15K2HEV5OHOHDRTAY2HG1XMUSQ&client_secret=NLRNJD0BN32HSXPIRQRMXDU5FYGYV4CVQ1LRWUV15UGOORDR&v=20190425";
        public Money money;
        private bool isFirstGetLocation = true;
        public TodayPage()
        {
            InitializeComponent();
            places = new ObservableCollection<Place>();
            RecomendationCards.ItemsSource = places;
        }
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
                //indicator.IsRunning = false;
                isFirstGetLocation = false;
            }
        }
        const string weatherUrlPrefix = "https://ru.api.openweathermap.org/data/2.5/weather?";//lat=35&lon=139
        const string weatherUrlSafix = "&units=metric&lang=ru&appid=339905ee10b8292e3cf6d11569aacf96";
        WeatherInfo weatherInfo;
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
        public string GetStringTemperarure(int temp)
        {
            return temp > 0 ? "+" + temp + "°" : temp + "°";
        }
        public string UpFirstChar(string s)
        {
            var safix = s.Substring(1);

            return char.ToUpper(s[0]) + safix;
        }
        void WeatherUpdate_Clicked(System.Object sender, System.EventArgs e)
        {
            weatherUpdate = true;
            GetLocation(DoWeather);
        }
        bool weatherUpdate = false;
        // TODO Исправить косяк с задержкой при запросе:
        //      1. Использовать другое Api
        //      !2. Кешировать
        //      3. при долдгом запросе просто выводить сообщение об ошибке
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
                            Console.WriteLine("DONE");
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
        async private Task DoReq()
        {
            if (location != null)
            {
                string safix = $"&ll={GetDouble(location.Latitude)},{GetDouble(location.Longitude)}&categoryId=4d4b7104d754a06370d81259&limit=10&radius=2000";

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
        private void MakePlacesList(PlacesInfo placesInfo)
        {
            foreach (var groups in placesInfo.response.groups)
            {
                foreach (var items in groups.items)
                {
                    Place newPlace = new Place(items.venue) { TripId = MainPage.CurrentTrip.Id };
                    places.Add(newPlace);
                }
            }
        }
        ObservableCollection<Place> places;
        private string GetDouble(double d)
        {
            string req = d.ToString();
            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                req = req.Replace(',', '.');
            }
            return req;
        }

        Position pLocation;
        Xamarin.Essentials.Location location;
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
                Console.WriteLine("AAAA" + ex.Message);

            }
        }

        private async void GetGeolocation()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        MessagingCenter.Send(this, "LocationAlert");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    pLocation = await CrossGeolocator.Current.GetPositionAsync();
                    DoReq();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                    MessagingCenter.Send(this, "LocationDenied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
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

        async void OnPlaceSelected(Object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Place;
            if (item == null)
            {

                RecomendationCards.SelectedItem = null;
                return;
            }
            await Navigation.PushAsync(new PlaceDatailsPage(item));
            RecomendationCards.SelectedItem = null;
        }

        async void FinanceWidget_Tapped(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            await Navigation.PushAsync(new FinancePage());
        }
        async void RecomedationSaved_Clicked(System.Object sender, System.EventArgs e)
        {
            await recomedationSavedButton.FadeTo(0.4, 100);
            await Task.Delay(50);
            await recomedationSavedButton.FadeTo(1, 100);
            await Navigation.PushModalAsync(new NavigationPage(new RecomedationSavedPage()));
        }


    }
}

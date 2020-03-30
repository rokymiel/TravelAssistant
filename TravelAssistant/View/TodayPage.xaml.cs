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

namespace TravelAssistant.View
{
    public partial class TodayPage : ContentPage
    {
        string recUrl = "https://api.foursquare.com/v2/venues/explore?client_id=GMDFIDPK1TFXA45NFQVXDI15K2HEV5OHOHDRTAY2HG1XMUSQ&client_secret=NLRNJD0BN32HSXPIRQRMXDU5FYGYV4CVQ1LRWUV15UGOORDR&v=20190425";
        public Money money;
        public TodayPage()
        {

            InitializeComponent();
            places = new ObservableCollection<Place>();
            RecomendationCards.ItemsSource = places;
            

            //GetLocation();

        }
        private string GetCurrentDate()
        {
            var date = DateTime.Now;
            return date.Day + " " + date.ToString("dddd");
        }
        private bool isFirstGetLocation = true;
        protected override void OnAppearing()
        {
            date.Text = GetCurrentDate();
            money = App.moneyManager.GetMoney();
            if (money != null)
            {
                allMoneyLabel.Text = money.AllMoney.ToString();
                currentMoneyLabel.Text = money.CurrentMoney.ToString();
                if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);
            }
            if (isFirstGetLocation)
            {
                indicator.IsRunning = true;
                GetLocationAndRecomendation();
                //indicator.IsRunning = false;
                isFirstGetLocation = false;
            }
        }
        private void DoReq()
        {
            if (location != null)
            {
                string safix = $"&ll={GetDouble(location.Latitude)},{GetDouble(location.Longitude)}&categoryId=4d4b7104d754a06370d81259&limit=10&radius=2000";
                recUrl += safix;
                try
                {
                    PlacesInfo placesInfo = GetPlacesRecomendation(recUrl);
                    //Console.WriteLine(placesInfo.ToString());
                    if (placesInfo.meta.code == 200)
                    {
                        indicator.IsRunning = false;
                        MakePlacesList(placesInfo);
                    }
                    else
                    {
                        DisplayAlert("Ошибка", $"Код: {placesInfo.meta.code}", "OK");
                    }
                }
                catch (WebException ex)
                {
                    DisplayAlert("Ошибка", "Проблемы с интернетом", "OK");
                }
                catch (Exception)
                {
                    DisplayAlert("Ошибка", "Произошла непредвиденная ошибка", "OK");
                }

            }
        }
        private void MakePlacesList(PlacesInfo placesInfo)
        {
            foreach (var groups in placesInfo.response.groups)
            {
                foreach (var items in groups.items)
                {
                    Place newPlace = new Place(items.venue);
                    //newPlace.Id= Guid.NewGuid().ToString(); ;
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
        private async void GetLocationAndRecomendation()
        {
            try
            {

                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium);

                location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude:f5}, Longitude: {location.Longitude:f5}, Altitude: {location.Altitude}");
                    DoReq();
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
                await DisplayAlert("Ошибка", "Не удается определить местоположение", "OK");
                //Console.WriteLine("AAAA" + ex.Message);

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
                    //GeolocationField = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                    MessagingCenter.Send(this, "LocationDenied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //GeolocationField = "Error: " + ex;
            }
        }
        private PlacesInfo GetPlacesRecomendation(string url)
        {

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            PlacesInfo placesInfo = JsonConvert.DeserializeObject<PlacesInfo>(response);
            return placesInfo;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {

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
    }
}

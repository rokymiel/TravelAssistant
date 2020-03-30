using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TravelAssistant.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TravelAssistant.View
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            placesPin = new ObservableCollection<PinLocation>();
            //map.ItemsSource = placesPin;
            
        }
        
        ObservableCollection<PinLocation> placesPin;
        protected override void OnAppearing()
        {
            placesPin = new ObservableCollection<PinLocation>();
            App.placesManager.GetPlaces().ForEach(x=> placesPin.Add(new PinLocation(x.Address,x.Name,new Position(x.Lat,x.Lng))));
            map.ItemsSource = placesPin;
            GetLocation();
            
        }
        private void DoSome()
        {
            Position position = new Position(location.Latitude, location.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            map.MoveToRegion(mapSpan);
        }
        Xamarin.Essentials.Location location;
        private async void GetLocation()
        {
            try
            {

                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium);
                location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude:f5}, Longitude: {location.Longitude:f5}, Altitude: {location.Altitude}");
                    DoSome();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Unable to get location
            }
        }
        public class PinLocation : INotifyPropertyChanged
        {
            Position _position;

            public string Address { get; }
            public string Description { get; }

            public Position Position
            {
                get => _position;
                set
                {
                    if (!_position.Equals(value))
                    {
                        _position = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
                    }
                }
            }

            public PinLocation(string address, string description, Position position)
            {
                Address = address;
                Description = description;
                Position = position;
            }

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }

        void Pin_InfoWindowClicked(System.Object sender, Xamarin.Forms.Maps.PinClickedEventArgs e)
        {
            
            //Console.WriteLine((sender as Pin).Label);
        }

        void map_MapClicked(System.Object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            
            infoView.IsVisible = false;
        }

        async void Pin_MarkerClicked(System.Object sender, Xamarin.Forms.Maps.PinClickedEventArgs e)
        {
            e.HideInfoWindow = true;
            infoView.Opacity = 0;
            infoView.IsVisible = true;
            infoName.Text=(sender as Pin).Label;
            infoAddress.Text = (sender as Pin).Address;
            await infoView.FadeTo(1,200);
        }
    }
}

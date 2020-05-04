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
        /// <summary>
        /// Список сохраненных мест.
        /// </summary>
        ObservableCollection<PinLocation> placesPin;
        Location location;
        public MapPage()
        {
            InitializeComponent();
            placesPin = new ObservableCollection<PinLocation>();

        }
        protected override void OnAppearing()
        {
            placesPin = new ObservableCollection<PinLocation>();
            App.placesManager.GetTripItems<Place>(MainPage.CurrentTrip.Id).ForEach(x => placesPin.Add(new PinLocation(x.Address, x.Name, new Position(x.Lat, x.Lng))));
            map.ItemsSource = placesPin;
            GetLocation(DoSome);

        }
        private void DoSome()
        {
            Position position = new Position(location.Latitude, location.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            map.MoveToRegion(mapSpan);
        }
        
        private async void GetLocation(Action action)
        {
            try
            {

                var request = new GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    action();
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

        void map_MapClicked(System.Object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            infoView.IsVisible = false;
        }

        async void Pin_MarkerClicked(System.Object sender, Xamarin.Forms.Maps.PinClickedEventArgs e)
        {
            e.HideInfoWindow = true;
            
            // Перемещение пина к центру карты.
            GetLocation(delegate
            {
                var pin = sender as Pin;
                
                MapSpan mapSpan = new MapSpan(pin.Position, map.VisibleRegion.LatitudeDegrees, map.VisibleRegion.LongitudeDegrees);
                map.MoveToRegion(mapSpan);

            });
            infoView.Opacity = 0;
            infoView.IsVisible = true;
            infoName.Text = (sender as Pin).Label;
            infoAddress.Text = (sender as Pin).Address;
            await infoView.FadeTo(1, 200);
        }
    }
}

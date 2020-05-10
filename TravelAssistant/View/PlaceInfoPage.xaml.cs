using System;
using System.Collections.Generic;
using TravelAssistant.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TravelAssistant.View
{
    public partial class PlaceInfoPage : ContentPage
    {
        /// <summary>
        /// Выбранное  место.
        /// </summary>
        Place Place { get; set; }

        public PlaceInfoPage(Place place)
        {
            InitializeComponent();
            Place = place;
            detailsLayout.BindingContext = Place;
            Position position = new Position(place.Lat, place.Lng);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            placeMap.MoveToRegion(mapSpan);
            placePin.Label = place.Name;
            placePin.Position = position;
            CategoriesCollection.ItemsSource = new List<Categories>(Place.Categories);
        }
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {

            if (!App.placesManager.ContainsPlace(Place))
            {
                App.placesManager.AddItem(Place);
            }
        }
    }
}

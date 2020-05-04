using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelAssistant.Managers;
using TravelAssistant.Models;
using TravelAssistant.View;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace TravelAssistant
{
    public partial class AllTripsPage : ContentPage
    {
        /// <summary>
        /// Список поездок.
        /// </summary>
        public ObservableCollection<Trip> trips;
        public AllTripsPage()
        {
            trips = new ObservableCollection<Trip>();
            InitializeComponent();
            App.tripsManager.GetItems().ForEach(x => trips.Add(x));
            allTripsView.ItemsSource = trips;
        }
        async void OnNewTrip_Tapped(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            await Navigation.PushAsync(new CreateNewTripPage());
        }

        async void OnTrip_Tapped(System.Object sender, System.EventArgs e)
        {
            var item = (Trip)(sender as PancakeView).BindingContext;
            if (item != null)
            {
                (sender as PancakeView).IsEnabled = false;
                await AnimationManager.StartScalePancakeView(sender as PancakeView);
                (sender as PancakeView).IsEnabled = true;
                App.Current.Properties["mainPage"] = item.Id;
                App.Current.MainPage = new MainPage(item);
                return;
            }
            await AnimationManager.StartScalePancakeView(sender);
            
        }
    }
}

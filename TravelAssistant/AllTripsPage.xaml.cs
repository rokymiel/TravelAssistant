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
        ObservableCollection<Trip> trips;
        public AllTripsPage()
        {
            //SetValue(NavigationPage.HasNavigationBarProperty, false);
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

        void allTripsView_Scrolled(System.Object sender, Xamarin.Forms.ItemsViewScrolledEventArgs e)
        {
            //Console.WriteLine(e.VerticalOffset);
            //if (e.VerticalOffset > nameLay.Height)
            //{
            //    SetValue(NavigationPage.HasNavigationBarProperty, true);
            //}
            //else
            //{
            //    SetValue(NavigationPage.HasNavigationBarProperty, false);
            //}
        }
    }
}

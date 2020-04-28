using System;
using System.Collections.Generic;
using TravelAssistant.Managers;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class TripDetailsPage : ContentPage
    {
        public TripDetailsPage()
        {
            InitializeComponent();
            tripLayout.BindingContext = MainPage.CurrentTrip;
            beginingLabel.Text = MainPage.CurrentTrip.BeginingDate.ToString("Начало: d MMMM yyyy");
            endingLabel.Text = MainPage.CurrentTrip.EndingDate.ToString("Конец: d MMMM yyyy");
        }

        async void ExitTrip_Tapped(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            if (App.Current.Properties.ContainsKey("mainPage"))
                App.Current.Properties.Remove("mainPage");
            App.Current.MainPage = new NavigationPage(new AllTripsPage());
        }
    }
}

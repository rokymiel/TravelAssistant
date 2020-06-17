using System;
using System.Collections.Generic;
using TravelAssistant.Managers;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace TravelAssistant.View
{
    public partial class OtherPage : ContentPage
    {
        public OtherPage()
        {
            InitializeComponent();
        }

        async void OnFinanceTabbed(System.Object sender, System.EventArgs e)
        {
            (sender as PancakeView).IsEnabled = false;
            await AnimationManager.StartScalePancakeView(sender as PancakeView);
            await Navigation.PushAsync(new FinancePage());
            (sender as PancakeView).IsEnabled = true;
        }

        async void OnTrip_Tapped(System.Object sender, System.EventArgs e)
        {
            (sender as PancakeView).IsEnabled = false;
            await AnimationManager.StartScalePancakeView(sender as PancakeView);
            await Navigation.PushAsync(new TripDetailsPage());
            (sender as PancakeView).IsEnabled = true;
        }

        async void Exit_Tapped(System.Object sender, System.EventArgs e)
        {
            (sender as PancakeView).IsEnabled = false;
            await AnimationManager.StartScalePancakeView(sender as PancakeView);
            if (App.Current.Properties.ContainsKey("mainPage"))
                App.Current.Properties.Remove("mainPage");
            App.Current.MainPage = new NavigationPage(new AllTripsPage());
        }

        async void AboutApp_Tapped(System.Object sender, System.EventArgs e)
        {
            (sender as PancakeView).IsEnabled = false;
            await AnimationManager.StartScalePancakeView(sender as PancakeView);
            await Navigation.PushAsync(new AboutAppPage());
            (sender as PancakeView).IsEnabled = true;
        }
    }
}

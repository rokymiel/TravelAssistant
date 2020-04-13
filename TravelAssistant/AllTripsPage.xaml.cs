using System;
using System.Collections.Generic;
using TravelAssistant.Managers;
using TravelAssistant.View;
using Xamarin.Forms;

namespace TravelAssistant
{
    public partial class AllTripsPage : ContentPage
    {
        public AllTripsPage()
        {
            InitializeComponent();
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            await Navigation.PushAsync(new CreateNewTripPage());
        }
    }
}

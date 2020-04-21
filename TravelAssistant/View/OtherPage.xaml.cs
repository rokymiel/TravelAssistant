﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

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
            await Navigation.PushAsync(new FinancePage());
        }

        void OnTrip_Tapped(System.Object sender, System.EventArgs e)
        {

        }

        void Exit_Tapped(System.Object sender, System.EventArgs e)
        {
            if (App.Current.Properties.ContainsKey("mainPage"))
                App.Current.Properties.Remove("mainPage");
            App.Current.MainPage = new NavigationPage(new AllTripsPage());
        }
    }
}

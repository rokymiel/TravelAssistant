using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class CreateNewTripPage : ContentPage
    {
        public CreateNewTripPage()
        {
            InitializeComponent();
        }

         void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new MainPage();
            //await Navigation.PushModalAsync(new MainPage());
            //await Navigation.PopToRootAsync();
        }
    }
}

using System;
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
    }
}

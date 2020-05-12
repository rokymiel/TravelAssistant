using System;
using System.Collections.Generic;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class CreateNewTripPage : ContentPage
    {
        public CreateNewTripPage()
        {
            InitializeComponent();
        }
        public bool CheckEntry(Entry entry, string message)
        {
            if (string.IsNullOrEmpty(entry.Text))
            {
                DisplayAlert("Ошибка", message, "OK");
                return false;
            }
            return true;
        }
        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!CheckEntry(countryOfTripEntry, "Не указано название страны") || !CheckEntry(cityOfTripEntry, "Не указано название города"))
            {
                return;
            }
            if (beginingOfTripDatePicker.Date > endingOfTripDatePicker.Date)
            {
                await DisplayAlert("Ошибка", "Неверно указана дата начала и окончания поездки", "OK");
                return;
            }
            Trip newTrip = new Trip(countryOfTripEntry.Text, cityOfTripEntry.Text,
                beginingOfTripDatePicker.Date, endingOfTripDatePicker.Date)
            { Id = Guid.NewGuid().ToString() };
            App.tripsManager.AddItem(newTrip);
            App.Current.Properties["mainPage"] = newTrip.Id;
            App.Current.MainPage = new MainPage(newTrip);
        }
    }
}

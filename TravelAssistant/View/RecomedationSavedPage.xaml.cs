using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class RecomedationSavedPage : ContentPage
    {
        ObservableCollection<Place> places;
        public RecomedationSavedPage()
        {
            InitializeComponent();
            places = new ObservableCollection<Place>();
            App.placesManager.GetPlaces().ForEach(x=>places.Add(x));
            RecomendationSavedCards.ItemsSource = places;
            if (places.Count == 0)
            {
                noSavesLabel.IsVisible = true;
            }
        }

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Delete_Clicked(System.Object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Удалить сохраненное место?", "Сохранение будет удалено, данное действие необратимо.", "Удалить", "Отмена"))
            {
                var place = (Place)(sender as SwipeItemView).CommandParameter;
                places.Remove(place);
                App.placesManager.DeleteItem(place);
            }
        }
    }
}

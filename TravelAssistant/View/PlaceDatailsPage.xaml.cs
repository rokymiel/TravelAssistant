﻿using System;
using System.Collections.Generic;
using TravelAssistant.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Collections.ObjectModel;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Transformations;

namespace TravelAssistant.View
{
    public partial class PlaceDatailsPage : ContentPage
    {
        /// <summary>
        /// Выбранное  место.
        /// </summary>
        Place Place { get; set; }
        PlaceDatails PlaceDatails { get; set; }
        public PlaceDatailsPage(Place place)
        {
            InitializeComponent();
            Place = place;
            detailsLayout.BindingContext = Place;
            Position position = new Position(place.Lat, place.Lng);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            placeMap.MoveToRegion(mapSpan);
            placePin.Label = place.Name;
            placePin.Position = position;
            CategoriesCollection.ItemsSource = new List<Categories>(Place.Categories);
            GetPlaceDatailsAsync($"https://api.foursquare.com/v2/venues/{Place.PlaceId}?client_id=GMDFIDPK1TFXA45NFQVXDI15K2HEV5OHOHDRTAY2HG1XMUSQ&client_secret=NLRNJD0BN32HSXPIRQRMXDU5FYGYV4CVQ1LRWUV15UGOORDR&v=20190425&locale=ru");

        }
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {

            if (!App.placesManager.ContainsPlace(Place))
            {
                App.placesManager.AddItem(Place);
            }
        }
        async void SetImage()
        {
            if (PlaceDatails.response.venue.bestPhoto != null)
            {
                if (!string.IsNullOrEmpty(PlaceDatails.response.venue.bestPhoto.prefix) && !string.IsNullOrEmpty(PlaceDatails.response.venue.bestPhoto.suffix))
                {
                    try
                    {
                        Uri uri = new Uri(PlaceDatails.response.venue.bestPhoto.prefix + "original" + PlaceDatails.response.venue.bestPhoto.suffix);
                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            
                            placeImage.Source = ImageSource.FromUri(uri);
                            placeImage.IsVisible = true;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            var byteArray = await new WebClient().DownloadDataTaskAsync(uri);
                            placeImage.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            placeImage.IsVisible = true;
                        }

                        
                    }
                    catch (Exception)
                    {
                        placeImage.IsVisible = false;
                    }
                }
            }
        }
        void SetDetails()
        {
            if (!string.IsNullOrEmpty(PlaceDatails.response.venue.description))
            {
                descriptionLabel.IsVisible = true;
                descriptionView.IsVisible = true;
                description.Text = PlaceDatails.response.venue.description;
            }
            if (!string.IsNullOrEmpty(PlaceDatails.response.venue.url))
            {
                contactsLabel.IsVisible = true;
                contactsView.IsVisible = true;
                urlLabel.IsVisible = true;
                urlText.IsVisible = true;
                urlText.Text = PlaceDatails.response.venue.url;
            }
            if (!string.IsNullOrEmpty(PlaceDatails.response.venue.contact.formattedPhone))
            {
                contactsLabel.IsVisible = true;
                contactsView.IsVisible = true;
                phoneLabel.IsVisible = true;
                phoneText.IsVisible = true;
                phoneText.Text = PlaceDatails.response.venue.contact.formattedPhone;
            }
            if (!string.IsNullOrEmpty(PlaceDatails.response.venue.contact.twitter))
            {
                contactsLabel.IsVisible = true;
                contactsView.IsVisible = true;
                twLabel.IsVisible = true;
                twText.IsVisible = true;
                twText.Text = PlaceDatails.response.venue.contact.twitter;
            }
            if (!string.IsNullOrEmpty(PlaceDatails.response.venue.contact.instagram))
            {
                contactsLabel.IsVisible = true;
                contactsView.IsVisible = true;
                instLabel.IsVisible = true;
                instText.IsVisible = true;
                instText.Text = PlaceDatails.response.venue.contact.instagram;
            }
            SetImage();

        }
        async void GetPlaceDatailsAsync(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse2 = (HttpWebResponse)await httpWebRequest2.GetResponseAsync();
                string response2;
                using (StreamReader streamReader = new StreamReader(httpWebResponse2.GetResponseStream()))
                {
                    response2 = streamReader.ReadToEnd();
                }
                PlaceDatails = JsonConvert.DeserializeObject<PlaceDatails>(response2);
                SetDetails();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", "Возникла ошибка при получении детальной информации о месте", "Ок");
            }
        }

    }
}

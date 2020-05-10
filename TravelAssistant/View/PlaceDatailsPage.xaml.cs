using System;
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
            //PlaceDatails = GetPlaceDatails();
            //SetDetails();
            GetPlaceDatailsAsync($"https://api.foursquare.com/v2/venues/{Place.PlaceId}?client_id=GMDFIDPK1TFXA45NFQVXDI15K2HEV5OHOHDRTAY2HG1XMUSQ&client_secret=NLRNJD0BN32HSXPIRQRMXDU5FYGYV4CVQ1LRWUV15UGOORDR&v=20190425&locale=ru");

        }
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {

            if (!App.placesManager.ContainsPlace(Place))
            {
                App.placesManager.AddItem(Place);
            }
        }
        void SetImage()
        {
            if (PlaceDatails.response.venue.bestPhoto != null)
            {
                if (!string.IsNullOrEmpty(PlaceDatails.response.venue.bestPhoto.prefix) && !string.IsNullOrEmpty(PlaceDatails.response.venue.bestPhoto.suffix))
                {
                    try
                    {
                        Uri uri = new Uri(PlaceDatails.response.venue.bestPhoto.prefix + "original" + PlaceDatails.response.venue.bestPhoto.suffix);
                        placeImage.Source = ImageSource.FromUri(uri);
                        placeImage.IsVisible = true;
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
        PlaceDatails GetPlaceDatails()
        {
            return new PlaceDatails()
            {
                response = new PlaceResponse()
                {
                    venue = new VenueDetails()
                    {
                        id = "asa",
                        name = "ExapleName",
                        contact = new Contact()
                        {
                            formattedPhone = "+7977754265",
                            twitter = "chsjdhyr",
                            instagram = "hgdsghfbhs"
                        },
                        url = "http://mem.ru",
                        description = "Это длинное описание данного места, что здесь писать я не знаю но напишу потому что это важно для меня прям очень",
                        rating = 8.6,
                        bestPhoto = new BestPhoto()
                        {
                            prefix = "https://fastly.4sqi.net/img/general/",
                            suffix = "/16771017_d3UR1Quu8NOzjt6IIR5-JVf4B_GtY2xbrtYhu8Rgg54.jpg"
                        }

                    }
                }
            };
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
                await DisplayAlert("Ошибка", url + "  " + ex.Message, "Ок");
            }
        }

    }
}

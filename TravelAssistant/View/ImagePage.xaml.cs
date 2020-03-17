using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class ImagePage : ContentPage
    {
        public ImagePage(ImageSource source)
        {
            InitializeComponent();
            if (source == null) Console.WriteLine("NUUUUUULLLLL");
            Console.WriteLine((source as ImageSource));
            image.Source = source;
        }


        async void SwipeGestureRecognizer_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            
            await Navigation.PopModalAsync();
        }
    }
}

using System;
using Xamarin.Forms;

namespace TravelAssistant.Models
{
    public class ImageEventArgs:EventArgs
    {
        public ImageEventArgs(ImageSource imageSource)
        {
            Image = imageSource;
        }
        public ImageSource Image { get; set; }
    }
}

using System;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace TravelAssistant.Models
{
    public class ImageEventArgs:EventArgs
    {
        public ImageEventArgs(ImageSource imageSource)
        {
            Image = imageSource;
        }
        public ImageEventArgs(string path) { Path = path; }
        public ImageEventArgs() { }
        public ImageSource Image { get; set; }
        public string Path { get; set; }
    }
}

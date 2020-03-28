using System;
using Plugin.Media.Abstractions;
using SQLite;
using Xamarin.Forms;

namespace TravelAssistant.Models
{
    public class Document:Item
    {
        public string Path { get; set; }
        [Ignore]
        public ImageSource Image { get; set; }
        
    }
}

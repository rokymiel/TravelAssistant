using System;
using System.IO;
using Plugin.Media.Abstractions;
using SQLite;
using Xamarin.Forms;

namespace TravelAssistant.Models
{
    public class Document : TripData
    {
        public string Path { get; set; }
        public byte[] ByteImage { get; set; }
        [Ignore]
        public ImageSource Image { get => ImageSource.FromStream(() => new MemoryStream(ByteImage));  }

    }
}

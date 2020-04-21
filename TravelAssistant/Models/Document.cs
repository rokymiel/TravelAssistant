using System;
using System.IO;
using Plugin.Media.Abstractions;
using SQLite;
using Xamarin.Forms;

namespace TravelAssistant.Models
{
    /// <summary>
    /// Документ пользователя.
    /// </summary>
    public class Document : TripData
    {
        /// <summary>
        /// Путь полученного изображения.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Изображение в byte.
        /// </summary>
        public byte[] ByteImage { get; set; }
        /// <summary>
        /// Источник изображения для отображения.
        /// </summary>
        [Ignore]
        public ImageSource Image { get => ImageSource.FromStream(() => new MemoryStream(ByteImage));  }

    }
}

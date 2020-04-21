using System;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace TravelAssistant.Models
{
    public class ImageEventArgs : EventArgs
    {
        public ImageEventArgs(ImageSource imageSource)
        {
            Image = imageSource;
        }
        public ImageEventArgs(string path) { Path = path; }
        public ImageEventArgs() { }
        /// <summary>
        /// Выбранное изображение.
        /// </summary>
        public ImageSource Image { get; set; }
        /// <summary>
        /// Путь изображения.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Изображение в битах.
        /// </summary>
        public byte[] ByteImage { get; set; }
    }
}

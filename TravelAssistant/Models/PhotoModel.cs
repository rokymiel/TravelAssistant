using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Xamarin.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;

namespace TravelAssistant.Models
{
    /// <summary>
    /// Работает с изображениями. Получение изображения из библиотеки на телефоне.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class PhotoModel
    {
        public PhotoModel()
        {
            GetImageAndRunCommand = new Command(GetImageAndRun);
        }
        /// <summary>
        /// Событие получение изображения.
        /// </summary>
        public EventHandler<ImageEventArgs> GetImage;
        /// <summary>
        /// Источник изображения.
        /// </summary>
        public ImageSource ImageSource { get; set; }

        public ICommand GetImageAndRunCommand { get; set; }
        /// <summary>
        /// Получение изображения с устройства.
        /// </summary>
        public async void GetImageAndRun()
        {
            try
            {
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
                }
                ImageEventArgs args = new ImageEventArgs();
                if (storageStatus == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();

                    var file = await CrossMedia.Current.PickPhotoAsync();
                    if (file == null)
                        return;
                    byte[] imageArray = null;
                    if (file != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var stream = file.GetStream();
                            stream.CopyTo(ms);
                            imageArray = ms.ToArray();
                        }
                    }
                    ImageSource = ImageSource.FromFile(file.Path);
                    args.Path = file.Path;
                    args.Image = ImageSource;
                    args.ByteImage = imageArray;
                    file.Dispose();
                    GetImage?.Invoke(this, args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

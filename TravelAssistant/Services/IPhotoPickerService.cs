using System;
using System.IO;
using System.Threading.Tasks;

namespace TravelAssistant.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}

using System;
using Xamarin.Essentials;

namespace TravelAssistant.Managers
{
    public class FileAccessManager
    {
        public static string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }

    }
}

using Lain.Core;
using System.IO;
using Windows.ApplicationModel;

namespace Lain.StoreApp
{
    public class StoreAppFileSystem : FileSystem
    {
        protected override Stream LoadStream(string file)
        {
             return Package.Current.InstalledLocation.OpenStreamForReadAsync(file).Result;
        }
    }
}

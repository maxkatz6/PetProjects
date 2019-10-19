using Lain.Core;
using System.IO;

namespace Lain.Desktop
{
    public class DesktopFileSystem : FileSystem
    {
        protected override Stream LoadStream(string file)
        {
            return new FileStream(file, FileMode.Open);
        }
    }
}

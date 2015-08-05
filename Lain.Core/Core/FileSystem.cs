using System.IO;

namespace Lain.Core
{
    public abstract class FileSystem
    {
        public static string BaseDirectory { get; set; } = "Assets";
        public static FileSystem Current { private get; set; }
        
        protected abstract Stream LoadStream(string file);

        public static string LoadContent(string file)
        {
            using (var sr = new StreamReader(Current.LoadStream(file)))
                return sr.ReadToEnd();
        }

        public static Stream Load(string file)
        {
            return Current.LoadStream(file);
        }

        public static string GetPath(string fileName, params string[] pathDirs)
        {
            return Path.Combine(BaseDirectory, Path.Combine(pathDirs), fileName.Replace('/', '\\').Trim('\\'));
        }
    }

}

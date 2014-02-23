using System;
using System.IO;
using System.Text;
using Ormeli.Core.Patterns;

namespace Ormeli.Core
{
    public class FileManager : Disposable
    {
        private readonly FileStream _fileStream;

        public FileManager(string fileName, bool append = true)
        {
            _fileStream = new FileStream(fileName, append ? FileMode.OpenOrCreate : FileMode.Create);
        }

        public void WriteLine(string str)
        {
            if (_fileStream != null) _fileStream.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            else ErrorProvider.SendError(new NullReferenceException("\rCannon to write " + str + ".\r\n Stream Writer is not initialized!"));
        }

        public void WriteByteArray(byte[] bytes, int offset = 0, int length = -1)
        {
            if (length == -1) length = bytes.Length;
            _fileStream.Write(bytes, offset, length);
        }

        public static void WriteText(string fileName, string str)
        {
            using (var fm = new FileManager(fileName))
                fm.WriteLine(str);
        }

        public static void WriteByteArray(string fileName, byte[] byteArr, bool append = true, int offset = 0, int length = -1)
        {
            using (var fm = new FileManager(fileName, append))
                fm.WriteByteArray(byteArr, offset, length);
        }
    }
}

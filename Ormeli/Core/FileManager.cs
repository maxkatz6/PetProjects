using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ormeli.Core.Patterns;

namespace Ormeli.Core
{
    public class FileManager : Disposable
    {
        //TODO Под снос. Оставить только статические обертки над классами из .Net библиотеки.
        private readonly FileStream _fileStream;
        private readonly string _fileName;
        private static FileManager _fileManager;

        public FileManager(string fileName, bool append = true)
        {
            _fileStream = new FileStream(fileName, append ? FileMode.Append : FileMode.Create);
            _fileName = fileName;
        }

        public string ReadLine()
        {
            var sb = new StringBuilder();
            for (var c = (char)_fileStream.ReadByte(); c != '\0' & c != '\n' ; c = (char)_fileStream.ReadByte())
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public string ReadToEnd()
        {
            var sb = new StringBuilder();
            for (var c = (char)_fileStream.ReadByte(); c != '\0'; c = (char)_fileStream.ReadByte())
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public void Write<T>(T obj)
        {
            WriteByteArray(BinaryOperations.GetArray(obj));
        }

        public void Write<T>(T[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
                Write(objs[i]);
        }

        public void WriteLine(string str)
        {
            _fileStream.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
        }

        public void WriteByteArray(byte[] bytes, int offset = 0, int length = -1)
        {
            if (length == -1) length = bytes.Length;
            _fileStream.Write(bytes, offset, length);
        }

        public static void WriteText(string fileName, string str, bool append = true)
        {
            if (_fileManager == null || fileName != _fileManager._fileName) _fileManager = new FileManager(fileName, append);
           _fileManager.WriteLine(str);
        }

        public static void WriteByteArray(string fileName, byte[] byteArr, bool append = true, int offset = 0, int length = -1)
        {
            if (_fileManager == null || fileName != _fileManager._fileName) _fileManager = new FileManager(fileName, append);
            _fileManager.WriteByteArray(byteArr, offset, length);
        }

        public static string ReadLine(string fileName)
        {
            if (_fileManager == null || fileName != _fileManager._fileName) _fileManager = new FileManager(fileName);
            return _fileManager.ReadLine();
        }
    }
}

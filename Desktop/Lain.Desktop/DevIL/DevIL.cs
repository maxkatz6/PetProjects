using Lain.Core;
using Lain.GAPI;
using System;
using System.Runtime.InteropServices;

namespace Lain.Desktop
{
    public class DevIL
    {
        private const uint IMAGE_WIDTH = 0x0DE4;
        private const uint IMAGE_HEIGHT = 0x0DE5;
        private const int RGBA = 0x1908;
        private const int FLOAT = 0x1406;

        public static void Initialize()
        {
            Init();
            Texture.FromFileDel = file =>
            {
                int id = 0;
                GenImages(1, ref id);
                BindImage(id);
                if (!(LoadImage(file) && ConvertImage(RGBA, FLOAT)))
                {
                    ErrorProvider.SendError("DevIL.LoadTexture() : " + GetError() + " on load " + file + ".");
                    return Texture.Null;
                }
                int w = GetInteger(IMAGE_WIDTH);
                int h = GetInteger(IMAGE_HEIGHT);

                return Texture.Create(GetData(), w, h);
            };
        }

        [DllImport("DevIL.dll", EntryPoint = "ilInit")]
        private static extern void Init();
        [DllImport("DevIL.dll", EntryPoint = "ilGenImages")]
        private static extern void GenImages(int d, ref int id);
        [DllImport("DevIL.dll", EntryPoint = "ilBindImage")]
        private static extern void BindImage(int id);
        [DllImport("DevIL.dll", EntryPoint = "ilDeleteImage")]
        private static extern void DeleteImage(int id);
        [DllImport("DevIL.dll", EntryPoint = "ilLoadImage", CharSet = CharSet.Ansi)]
        private static extern bool LoadImage(string filename);
        [DllImport("DevIL.dll", EntryPoint = "ilGetInteger")]
        private static extern int GetInteger(uint enumValue);
        [DllImport("DevIL.dll", EntryPoint = "ilConvertImage")]
        private static extern bool ConvertImage(int formatEnum, int typeEnum);
        [DllImport("DevIL.dll")]
        private static extern void ilDeleteImages(uint num, ref uint handle);
        [DllImport("DevIL.dll", EntryPoint = "ilGetError", CallingConvention = CallingConvention.StdCall)]
        private static extern ErrorType GetError();
        [DllImport("DevIL.dll", EntryPoint = "ilGetData", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern IntPtr GetData();
        private enum ErrorType
        {
            NoError = 0x0000,
            InvalidEnum = 0x0501,
            OutOfMemory = 0x0502,
            FormatNotSupported = 0x0503,
            InternalError = 0x0504,
            InvalidValue = 0x0505,
            IllegalOperation = 0x0506,
            IllegalFileValue = 0x0507,
            IllegalFileHeader = 0x0508,
            InvalidParameter = 0x0509,
            CouldNotOpenFile = 0x050A,
            InvalidExtension = 0x050B,
            FileAlreadyExists = 0x050C,
            OutFormatSame = 0x050D,
            StackOverflow = 0x050E,
            StackUnderflow = 0x050F,
            InvalidConversion = 0x0510,
            BadDimensions = 0x0511,
            FileReadError = 0x0512,
            FileWriteError = 0x0512,
            GifError = 0x05E1,
            JpegError = 0x05E2,
            PngError = 0x05E3,
            TiffError = 0x05E4,
            MngError = 0x05E5,
            Jp2Error = 0x05E6,
            ExrError = 0x05E7,
            UnknownError = 0x05FF
        }
    }
}

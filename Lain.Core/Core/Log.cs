using System;
using System.Diagnostics;
using System.IO;

namespace Lain.Core
{
    public static class Log
    {
        public static StreamWriter Stream { get; set; }

        public static void Write(string s)
        {
            if (Config.IsDebug)
                Debug.WriteLine(s);
            else
                Stream?.WriteLine(s);
        }

        public static void SendError(string s, bool sendExp = false)
        {
            Write(s);
            if (sendExp) throw new Exception(s);
        }

        public static void SendError(Exception ex, bool sendExp = false)
        {
            if (sendExp) throw ex;
            SendError(ex.Source + "\r\r" + ex.Message + "\r\r");
        }
    }
}
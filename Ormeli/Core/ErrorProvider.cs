using System;

namespace Ormeli.Core
{
    public static class ErrorProvider
    {
        private static readonly FileManager FileManager = new FileManager("log.txt");

        public static void SendError(string s, bool sendExp = false)
        {
            if (Config.IsDebug)
                Console.WriteLine(s);
            else
                FileManager.WriteLine(s);

            if (sendExp) throw new Exception(s);
        }

        public static void SendError(Exception ex, bool sendExp = false)
        {
            SendError(ex.Source + ex.Message + "\r\r");
            if (sendExp) throw ex;
        }
    }
}

using System;
using System.Text;

namespace TDF.Core
{
    static public class ErrorProvider
    {
        /// <summary>
        /// Sends error.
        /// </summary>
        /// <param name="ex">The exception</param>
        public static void Send(Exception ex)
        {
            var message = new StringBuilder(ex.Message);
#if DEBUG
            WinAPI.MessageBox(message.ToString(), "Error");
#else

            Log.Write(message.ToString());
#endif
        }
    }
}
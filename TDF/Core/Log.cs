using System.IO;

namespace TDF.Core
{
    /// <summary>
    /// Log helper class
    /// </summary>
    public static class Log
    {
        static StreamWriter _sw;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            _sw = new StreamWriter("log.txt",false);
            _sw.WriteLine("Log started");

        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public static void Shutdown()
        {
            _sw.Dispose();
        }


        /// <summary>
        /// Writes text to log file.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Write(string text)
        {
            _sw.WriteLine(text);
        }
    }
}
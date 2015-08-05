using System;
using System.Diagnostics;
using System.IO;

namespace Lain.Core
{
	public static class ErrorProvider
	{
        public static StreamWriter FileManager { get; set; }

		public static void SendError(string s, bool sendExp = false)
		{
			if (Config.IsDebug)
				Debug.WriteLine(s);
			else if (FileManager != null)
				FileManager.WriteLine(s);
                
			if (sendExp) throw new Exception(s);
		}

		public static void SendError(Exception ex, bool sendExp = false)
		{
			SendError(ex.Source + "\r\r"+ex.Message + "\r\r");
			if (sendExp) throw ex;
		}
	}
}
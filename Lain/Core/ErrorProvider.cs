using System;
using System.IO;

namespace Lain.Core
{
	public static class ErrorProvider
	{
		private static readonly StreamWriter FileManager = new StreamWriter("log.txt");

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
			SendError(ex.Source + "\r\r"+ex.Message + "\r\r");
			if (sendExp) throw ex;
		}
	}
}
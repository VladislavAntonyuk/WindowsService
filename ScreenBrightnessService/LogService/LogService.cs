using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenBrightnessService.LogService
{
	class LogService
	{
		public static void Log(string message)
		{
			using var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
			sw.WriteLine(DateTime.Now + ": " + message);
			sw.Flush();
			sw.Close();
		}
	}
}

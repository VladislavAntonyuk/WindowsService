using System;
using System.IO;

namespace WindowsService.Services.LogService;

internal class LogService
{
    public static void Log(string message)
    {
        using var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
        sw.WriteLine(DateTime.Now + ": " + message);
        sw.Flush();
        sw.Close();
    }
}
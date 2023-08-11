using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace WindowsService.Services.ThemeService;

internal class ThemeService
{
    public static string? GetTheme()
    {
        try
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes");
            return key?.GetValue("CurrentTheme", null)?.ToString();
        }
        catch (Exception e)
        {
            LogService.LogService.Log(e.Message);
            return null;
        }
    }

    public static void SetTheme(string themePath)
    {
        try
        {
            Process.Start("rundll32.exe", $"themecpl.dll,OpenThemeAction {themePath}");
        }
        catch (Exception e)
        {
            LogService.LogService.Log(e.Message);
        }
    }
}
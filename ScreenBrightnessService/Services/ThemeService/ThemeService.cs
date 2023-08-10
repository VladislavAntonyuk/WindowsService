using Microsoft.Win32;
using System;

namespace ScreenBrightnessService.Services.ThemeService;

internal class ThemeService
{
    public static string GetTheme()
    {
        try
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes");
            return key is null ? string.Empty : key.GetValue("CurrentTheme", string.Empty).ToString();
        }
        catch (Exception e)
        {
            LogService.LogService.Log(e.Message);
            throw;
        }
    }

    public static void SetTheme(string themePath)
    {
        try
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes");
            if (key is not null)
            {
                key.SetValue("CurrentTheme", themePath);
            }
        }
        catch (Exception e)
        {
            LogService.LogService.Log(e.Message);
            throw;
        }
    }
}
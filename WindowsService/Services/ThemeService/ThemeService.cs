using Microsoft.Win32;
using System;
using Microsoft.Win32.TaskScheduler;

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

    public static void SetTheme(string username, string themePath)
    {
        try
        {
            using var taskDefinition = TaskService.Instance.NewTask();
            taskDefinition.RegistrationInfo.Author = "Vladislav Antonyuk";
            taskDefinition.Actions.Add(new ExecAction("powershell", $"start-process -filepath \"{themePath}\"; timeout /t 3; taskkill /im \"systemsettings.exe\" /f"));
            using var task = TaskService.Instance.RootFolder.RegisterTaskDefinition("WindowsService.ThemeService", taskDefinition, TaskCreation.CreateOrUpdate, username, null, TaskLogonType.None);
            task.Run();
        }
        catch (Exception e)
        {
            LogService.LogService.Log(e.Message);
        }
    }
}
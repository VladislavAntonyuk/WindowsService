using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WindowsService.Models;
using WindowsService.Services.LogService;
using WindowsService.Services.ThemeService;

namespace WindowsService.Workers;

public class ThemeWorker : BackgroundService
{
    private readonly IOptionsMonitor<ThemeConfiguration> _configuration;

    public ThemeWorker(IOptionsMonitor<ThemeConfiguration> configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            LogService.Log($"CurrentTheme: {ThemeService.GetTheme()}");
            var themes = _configuration.CurrentValue.Themes;
            if (themes.Count == 0)
            {
                LogService.Log("Theme configuration is empty");
            }
            else
            {
                var targetTheme = themes.FirstOrDefault(x => TimeOnly.FromDateTime(DateTime.Now).IsBetween(x.StartTime, x.EndTime));
                if (targetTheme is null)
                {
                    LogService.Log("Target Theme is empty");
                }
                else
                {
                    LogService.Log($"Target Theme is {targetTheme.Path}");
                    ThemeService.SetTheme(targetTheme.Path);
                }
            }

            await Task.Delay(60 * 1000, stoppingToken);
        }
    }
}
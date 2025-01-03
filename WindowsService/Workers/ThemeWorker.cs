﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WindowsService.Models;
using WindowsService.Services.LogService;
using WindowsService.Services.ThemeService;

namespace WindowsService.Workers;

public class ThemeWorker(IOptionsMonitor<ThemeConfiguration> configuration) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			if (configuration.CurrentValue.IsEnabled)
			{
				var currentTheme = ThemeService.GetTheme();
				LogService.Log($"CurrentTheme: {currentTheme}");
				var themes = configuration.CurrentValue.Themes;
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
						if (targetTheme.Path != currentTheme)
						{
							LogService.Log($"Target Theme is {targetTheme.Path}");
							ThemeService.SetTheme(configuration.CurrentValue.Username, targetTheme.Path);
						}
					}
				}
			}
			
			await Task.Delay(60 * 1000, stoppingToken);
		}
	}
}
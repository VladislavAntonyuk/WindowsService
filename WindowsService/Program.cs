using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsService.Models;
using WindowsService.Workers;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostBuilder, services) =>
{
    services.Configure<BrightnessConfiguration>(hostBuilder.Configuration.GetRequiredSection("BrightnessConfiguration"));
    services.Configure<ThemeConfiguration>(hostBuilder.Configuration.GetRequiredSection("ThemesConfiguration"));
    services.AddHostedService<BrightnessWorker>();
    services.AddHostedService<ThemeWorker>();
    services.AddHostedService<KeyListenerWorker>();
}).UseWindowsService();

builder.Build().Run();
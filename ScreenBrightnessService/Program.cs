using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScreenBrightnessService.Models;
using ScreenBrightnessService.Workers;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostBuilder, services) =>
{
    services.Configure<BrightnessConfiguration>(hostBuilder.Configuration.GetRequiredSection("Brightness"));
    services.Configure<ThemeConfiguration>(hostBuilder.Configuration.GetRequiredSection("Theme"));
    services.AddHostedService<BrightnessWorker>();
    services.AddHostedService<ThemeWorker>();
}).UseWindowsService();

builder.Build().Run();
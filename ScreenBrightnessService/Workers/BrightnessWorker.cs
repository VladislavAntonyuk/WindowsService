using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ScreenBrightnessService.Models;
using ScreenBrightnessService.Services.BatteryService;
using ScreenBrightnessService.Services.BrightnessService;
using ScreenBrightnessService.Services.LogService;

namespace ScreenBrightnessService.Workers;

public class BrightnessWorker : BackgroundService
{
    private readonly IOptionsMonitor<BrightnessConfiguration> _configuration;

    public BrightnessWorker(IOptionsMonitor<BrightnessConfiguration> configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var powerState = PowerState.GetPowerState();
            if (powerState.BatteryFlag == BatteryFlag.NoSystemBattery)
            {
                LogService.Log("No system battery");
                await StopAsync(stoppingToken);
            }
            else
            {
                var batteryBrightness = GetBatteryBrightness();
                var chargeBrightness = GetChargeBrightness();
                var currentBrightness = BrightnessService.GetBrightness();
                var desiredBrightness = powerState.ACLineStatus == AcLineStatus.Offline
                    ? batteryBrightness
                    : chargeBrightness;
                if (currentBrightness != desiredBrightness)
                {
                    BrightnessService.SetBrightness(desiredBrightness);
                    LogService.Log($"CurrentBrightness: {desiredBrightness}");
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private byte GetBatteryBrightness()
    {
        var batteryBrightness = _configuration.CurrentValue.Battery;
        return batteryBrightness <= 100 ? batteryBrightness : (byte)100;
    }

    private byte GetChargeBrightness()
    {
        var chargeBrightness = _configuration.CurrentValue.Charge;
        return chargeBrightness > 100 ? (byte)100 : chargeBrightness;
    }
}
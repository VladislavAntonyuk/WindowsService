using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WindowsService.Models;
using WindowsService.Services.BatteryService;
using WindowsService.Services.BrightnessService;
using WindowsService.Services.LogService;

namespace WindowsService.Workers;

public class BrightnessWorker(IOptionsMonitor<BrightnessConfiguration> configuration) : BackgroundService
{
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
	            if (configuration.CurrentValue.IsEnabled)
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
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private byte GetBatteryBrightness()
    {
        var batteryBrightness = configuration.CurrentValue.Battery;
        return batteryBrightness <= 100 ? batteryBrightness : (byte)100;
    }

    private byte GetChargeBrightness()
    {
        var chargeBrightness = configuration.CurrentValue.Charge;
        return chargeBrightness > 100 ? (byte)100 : chargeBrightness;
    }
}
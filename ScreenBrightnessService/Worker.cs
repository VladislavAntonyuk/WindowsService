using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ScreenBrightnessService.BatteryService;

namespace ScreenBrightnessService
{
	public class Worker : BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			const byte minBrightness = 0;
			const byte maxBrightness = 100;
			while (!stoppingToken.IsCancellationRequested)
			{
				var powerState = PowerState.GetPowerState();
				if (powerState.BatteryFlag == BatteryFlag.NoSystemBattery)
				{
					LogService.LogService.Log("No system battery");
					await StopAsync(stoppingToken);
				}
				else
				{
					var currentBrightness = BrightnessService.BrightnessService.GetBrightness();
					var desiredBrightness = powerState.ACLineStatus == AcLineStatus.Offline
						? minBrightness
						: maxBrightness;
					if (currentBrightness != desiredBrightness)
					{
						BrightnessService.BrightnessService.SetBrightness(desiredBrightness);
						LogService.LogService.Log($"CurrentBrightness: {desiredBrightness}");
					}
				}

				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
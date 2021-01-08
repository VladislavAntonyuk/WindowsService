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
			while (!stoppingToken.IsCancellationRequested)
			{
				var powerState = PowerState.GetPowerState();
				if (powerState.BatteryFlag == BatteryFlag.NoSystemBattery)
					await StopAsync(stoppingToken);
				else
					BrightnessService.BrightnessService.SetBrightnessLaptop(
						powerState.ACLineStatus == AcLineStatus.Offline
							? 0
							: 100);

				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
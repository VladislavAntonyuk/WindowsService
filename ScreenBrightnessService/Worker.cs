using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ScreenBrightnessService.BatteryService;

namespace ScreenBrightnessService
{
	public class Worker : BackgroundService
	{
		private readonly IConfiguration configuration;

		public Worker(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
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
					var batteryBrightness = GetBatteryBrightness();
					var chargeBrightness = GetChargeBrightness();
					var currentBrightness = BrightnessService.BrightnessService.GetBrightness();
					var desiredBrightness = powerState.ACLineStatus == AcLineStatus.Offline
						? batteryBrightness
						: chargeBrightness;
					if (currentBrightness != desiredBrightness)
					{
						BrightnessService.BrightnessService.SetBrightness(desiredBrightness);
						LogService.LogService.Log($"CurrentBrightness: {desiredBrightness}");
					}
				}

				await Task.Delay(1000, stoppingToken);
			}
		}

		byte GetBatteryBrightness()
		{
			var batteryBrightness = configuration.GetSection("Brightness").GetValue<byte>("Battery");
			return (batteryBrightness <= 100) ? batteryBrightness : (byte)100;
		}

		byte GetChargeBrightness()
		{
			var batteryBrightness = configuration.GetSection("Brightness").GetValue<byte>("Charge");
			return (batteryBrightness > 100) ? (byte)100 : batteryBrightness;
		}
	}
}
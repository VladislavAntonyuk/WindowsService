using System;
using System.Management;
using Microsoft.Win32;

namespace ScreenBrightnessService.BrightnessService
{
	internal class BrightnessService
	{
		public static byte GetBrightness()
		{
			var managementObjectSearcher = new ManagementObjectSearcher(new ManagementScope("root\\WMI"),
				new SelectQuery("WmiMonitorBrightness"));
			var objectCollection = managementObjectSearcher.Get();
			byte num = 0;
			using (var enumerator = objectCollection.GetEnumerator())
			{
				if (enumerator.MoveNext())
					num = (byte) enumerator.Current.GetPropertyValue("CurrentBrightness");
			}

			objectCollection.Dispose();
			managementObjectSearcher.Dispose();
			return num;
		}

		private static void SetBrightnessRegistry(byte targetBrightness)
		{
			try
			{
				var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes");
				var activePowerScheme = key.GetValue("ActivePowerScheme");
				key = key.OpenSubKey($@"{activePowerScheme}\7516b95f-f776-4464-8c53-06167f40cc99\aded5e82-b909-4619-9949-f5d71dac0bcb", true);
				key.SetValue("ACSettingIndex", targetBrightness, RegistryValueKind.DWord);
				key.SetValue("DCSettingIndex", targetBrightness, RegistryValueKind.DWord);
			}
			catch (Exception e)
			{
				LogService.LogService.Log(e.Message);
				throw;
			}
		}

		public static void SetBrightness(byte targetBrightness)
		{
			SetBrightnessRegistry(targetBrightness);
			var managementObjectSearcher = new ManagementObjectSearcher(new ManagementScope("root\\WMI"),
				new SelectQuery("WmiMonitorBrightnessMethods"));
			var objectCollection = managementObjectSearcher.Get();
			using (var enumerator = objectCollection.GetEnumerator())
			{
				if (enumerator.MoveNext())
					((ManagementObject) enumerator.Current).InvokeMethod("WmiSetBrightness", new object[]
					{
						uint.MaxValue,
						targetBrightness
					});
			}

			objectCollection.Dispose();
			managementObjectSearcher.Dispose();
		}
	}
}
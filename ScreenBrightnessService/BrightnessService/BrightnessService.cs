using System.Management;

namespace ScreenBrightnessService.BrightnessService
{
	internal class BrightnessService
	{
		public static void SetBrightnessLaptop(int brightness)
		{
			using var managementClass = new ManagementClass("WmiMonitorBrightnessMethods")
			{
				Scope = new ManagementScope(@"\\.\root\wmi")
			};
			using var instances = managementClass.GetInstances();
			var args = new object[] {1, brightness};
			foreach (var instance in instances)
				if (instance is ManagementObject managementObject)
					managementObject.InvokeMethod("WmiSetBrightness", args);
		}
	}
}
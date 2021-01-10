using System.Management;

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

		public static void SetBrightness(byte targetBrightness)
		{
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
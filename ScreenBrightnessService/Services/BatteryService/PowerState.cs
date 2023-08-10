using System;
using System.Runtime.InteropServices;

namespace ScreenBrightnessService.Services.BatteryService;

[StructLayout(LayoutKind.Sequential)]
public class PowerState
{
    public AcLineStatus ACLineStatus;
    public BatteryFlag BatteryFlag;

    // direct instantiation not intended, use GetPowerState.
    private PowerState()
    {
    }

    public static PowerState GetPowerState()
    {
        var state = new PowerState();
        if (GetSystemPowerStatusRef(state))
            return state;

        throw new ApplicationException("Unable to get power state");
    }

    [DllImport("Kernel32", EntryPoint = "GetSystemPowerStatus")]
    private static extern bool GetSystemPowerStatusRef(PowerState sps);
}
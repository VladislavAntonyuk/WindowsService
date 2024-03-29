﻿namespace WindowsService.Services.BatteryService;

public enum BatteryFlag : byte
{
    High = 1,
    Low = 2,
    Critical = 4,
    Charging = 8,
    NoSystemBattery = 128,
    Unknown = 255
}
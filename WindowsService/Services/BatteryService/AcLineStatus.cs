namespace WindowsService.Services.BatteryService;

public enum AcLineStatus : byte
{
    Offline = 0,
    Online = 1,
    Unknown = 255
}
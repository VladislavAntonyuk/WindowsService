using System;

namespace ScreenBrightnessService.Models;

public class Theme
{
    public required string Path { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
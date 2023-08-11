using System;

namespace WindowsService.Models;

public class Theme
{
    public required string Path { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
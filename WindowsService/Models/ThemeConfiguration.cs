using System.Collections.Generic;

namespace WindowsService.Models;

public class ThemeConfiguration
{
    public required string Username { get; set; }
    public List<Theme> Themes { get; set; } = new();
}
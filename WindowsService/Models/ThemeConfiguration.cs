using System.Collections.Generic;

namespace WindowsService.Models;

public class ThemeConfiguration
{
    public List<Theme> Themes { get; set; } = new();
}
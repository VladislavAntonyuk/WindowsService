using System.Collections.Generic;

namespace WindowsService.Models;

public class ThemeConfiguration : FeatureConfiguration
{
    public required string Username { get; set; }
    public List<Theme> Themes { get; set; } = new();
}
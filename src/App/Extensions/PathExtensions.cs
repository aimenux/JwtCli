using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace App.Extensions;

[ExcludeFromCodeCoverage]
public static class PathExtensions
{
    public static string GetSettingFilePath() => Path.GetFullPath(Path.Combine(GetDirectoryPath(), @"appsettings.json"));

    public static string GetDirectoryPath()
    {
        try
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location);
        }
        catch
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
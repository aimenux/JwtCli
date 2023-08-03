using System.Reflection;
using App.Commands;

namespace App.Configuration;

public class Settings
{
    public static class Cli
    {
        public const string ToolName = @"JwtCli";
        public const string Description = @"Generate/Validate/Decode JWT Tokens.";
        public static readonly string UserSecretsFile = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Microsoft\UserSecrets\JwtCli-UserSecrets\secrets.json";
        public static readonly string Version = GetInformationalVersion().Split("+").FirstOrDefault();
        
        private static string GetInformationalVersion()
        {
            return typeof(ToolCommand)
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;
        }
    }
    
    public static class ExitCode
    {
        public const int Ok = 0;
        public const int Ko = -1;
    }
}
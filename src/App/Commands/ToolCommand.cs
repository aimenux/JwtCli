using App.Configuration;
using App.Extensions;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = Settings.Cli.ToolName, Description = $"\n{Settings.Cli.Description}")]
[Subcommand(typeof(JwtGenerateCommand), typeof(JwtValidateCommand), typeof(JwtDecodeCommand))]
public class ToolCommand : AbstractCommand
{
    public ToolCommand(IConsoleService consoleService) : base(consoleService)
    {
    }

    [Option("-s|--settings", "Show settings information.", CommandOptionType.NoValue)]
    public bool ShowSettings { get; set; }
    
    [Option("-v|--version", "Show version information.", CommandOptionType.NoValue)]
    public bool ShowVersion { get; init; }

    protected override void Execute(CommandLineApplication app)
    {
        if (ShowSettings)
        {
            var settingFile = PathExtensions.GetSettingFilePath();
            var userSecretsFile = Settings.Cli.UserSecretsFile;
            ConsoleService.RenderSettingsFile(settingFile);
            ConsoleService.RenderUserSecretsFile(userSecretsFile);
        }
        else if (ShowVersion)
        {
            ConsoleService.RenderVersion(Settings.Cli.Version);
        }
        else
        {
            ConsoleService.RenderTitle(Settings.Cli.ToolName);
            app.ShowHelp();
        }
    }
}
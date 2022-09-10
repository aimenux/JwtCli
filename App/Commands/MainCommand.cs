using App.Extensions;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "JwtCli", FullName = "Generate/Validate JWT Tokens", Description = "Generate/Validate JWT Tokens.")]
[Subcommand(typeof(JwtGenerateCommand), typeof(JwtValidateCommand))]
[VersionOptionFromMember(MemberName = nameof(GetVersion))]
public class MainCommand : AbstractCommand
{
    public MainCommand(IConsoleService consoleService) : base(consoleService)
    {
    }

    [Option("-s|--settings", "Show settings information.", CommandOptionType.NoValue)]
    public bool ShowSettings { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        if (ShowSettings)
        {
            var filepath = PathExtensions.GetSettingFilePath();
            ConsoleService.RenderSettingsFile(filepath);
        }
        else
        {
            const string title = "JwtCli";
            ConsoleService.RenderTitle(title);
            app.ShowHelp();
        }
    }

    protected static string GetVersion() => GetVersion(typeof(MainCommand));
}
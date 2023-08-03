using App.Services.Console;
using App.Services.Security;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Decode", FullName = "Decode JWT", Description = "Decode JWT.")]
[HelpOption]
public class JwtDecodeCommand : AbstractCommand
{
    private readonly ISecurityService _securityService;

    public JwtDecodeCommand(ISecurityService securityService, IConsoleService consoleService) : base(consoleService)
    {
        _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    [Option("-t|--token", "Jwt token", CommandOptionType.SingleValue)]
    public string Token { get; init; }

    protected override void Execute(CommandLineApplication app)
    {
        ConsoleService.RenderStatus(() =>
        {
            var securityDetails = _securityService.DecodeJwtToken(Token);
            ConsoleService.RenderSecurityDetails(securityDetails);
        });
    }
}
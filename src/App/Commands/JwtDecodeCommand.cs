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

    [Option("-j|--jwt", "JWT", CommandOptionType.SingleValue)]
    public string Jwt { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        var securityDetails = _securityService.DecodeJwtToken(Jwt);
        ConsoleService.RenderSecurityDetails(securityDetails);
    }

    protected override bool HasValidOptions()
    {
        return !string.IsNullOrWhiteSpace(Jwt);
    }
}
using App.Services.Console;
using App.Services.Security;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Generate", FullName = "Generate JWT", Description = "Generate JWT.")]
[HelpOption]
public class JwtGenerateCommand : AbstractCommand
{
    private readonly ISecurityService _securityService;

    public JwtGenerateCommand(ISecurityService securityService, IConsoleService consoleService) : base(consoleService)
    {
        _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    [Option("-c|--certificate", "Certificate file", CommandOptionType.SingleValue)]
    public string Certificate { get; init; }

    [Option("-p|--password", "Certificate password", CommandOptionType.SingleValue)]
    public string Password { get; init; }

    [Option("-a|--audience", "Token audience", CommandOptionType.SingleValue)]
    public string Audience { get; init; } = "localhost";

    [Option("-i|--issuer", "Token issuer", CommandOptionType.SingleValue)]
    public string Issuer { get; init; } = "localhost";

    [Option("-k|--kid", "Token kid", CommandOptionType.SingleValue)]
    public string Kid { get; init; } = Guid.NewGuid().ToString("N");

    [Option("-tt|--token-type", "Token type", CommandOptionType.SingleValue)]
    public string TokenType { get; init; } = "jwt";

    [Option("-e|--expire-after", "Token expiration in minutes", CommandOptionType.SingleValue)]
    public int ExpireInMinutes { get; init; } = 5;

    [Option("-f|--file", "Parameters file", CommandOptionType.SingleValue)]
    public string ParametersFile { get; init; }

    protected override void Execute(CommandLineApplication app)
    {
        ConsoleService.RenderStatus(() =>
        {
            var parameters = BuildCertificateParameters();
            var token = _securityService.GenerateJwtToken(parameters);
            ConsoleService.RenderJwtToken(token, parameters);
            ConsoleService.CopyTextToClipboard(token);
        });
    }

    private SecurityParameters BuildCertificateParameters()
    {
        if (string.IsNullOrWhiteSpace(ParametersFile))
        {
            return new SecurityParameters
            {
                Certificate = Certificate,
                Password = Password,
                ExpireInMinutes = ExpireInMinutes,
                TokenType = TokenType,
                Audience = Audience,
                Issuer = Issuer,
                Kid = Kid
            };
        }

        return SecurityParameters.BuildFromFile(ParametersFile);
    }
}
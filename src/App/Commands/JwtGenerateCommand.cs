using System.ComponentModel.DataAnnotations;
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
    public string Certificate { get; set; }

    [Option("-p|--password", "Certificate password", CommandOptionType.SingleValue)]
    public string Password { get; set; }

    [Option("-a|--audience", "Token audience", CommandOptionType.SingleValue)]
    public string Audience { get; set; } = "localhost";

    [Option("-i|--issuer", "Token issuer", CommandOptionType.SingleValue)]
    public string Issuer { get; set; } = "localhost";

    [Option("-k|--kid", "Token kid", CommandOptionType.SingleValue)]
    public string Kid { get; set; } = Guid.NewGuid().ToString("D");

    [Option("-tt|--token-type", "Token type", CommandOptionType.SingleValue)]
    public string TokenType { get; set; } = "jwt";

    [Range(1, int.MaxValue)]
    [Option("-e|--expire-after", "Token expiration in minutes", CommandOptionType.SingleValue)]
    public int ExpireInMinutes { get; set; } = 5;

    [Option("-f|--file", "Parameters file", CommandOptionType.SingleValue)]
    public string ParametersFile { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = BuildCertificateParameters();
        var token = _securityService.GenerateJwtToken(parameters);
        ConsoleService.RenderJwtToken(token, parameters);
        ConsoleService.CopyTextToClipboard(token);
    }

    protected override bool HasValidOptions()
    {
        if (string.IsNullOrWhiteSpace(ParametersFile))
        {
            return File.Exists(Certificate)
                   && !string.IsNullOrWhiteSpace(TokenType)
                   && !string.IsNullOrWhiteSpace(Password);
        }

        return File.Exists(ParametersFile);
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
using System.ComponentModel.DataAnnotations;
using App.Services.Console;
using App.Services.Security;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Validate", FullName = "Validate JWT", Description = "Validate JWT.")]
[HelpOption]
public class JwtValidateCommand : AbstractCommand
{
    private readonly ISecurityService _securityService;

    public JwtValidateCommand(ISecurityService securityService, IConsoleService consoleService) : base(consoleService)
    {
        _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    [Required]
    [Option("-t|--token", "Jwt token", CommandOptionType.SingleValue)]
    public string Token { get; init; }

    [Option("-c|--certificate", "Certificate file", CommandOptionType.SingleValue)]
    public string Certificate { get; init; }

    [Option("-p|--password", "Certificate password", CommandOptionType.SingleValue)]
    public string Password { get; init; }

    [Option("-a|--audience", "Token audience", CommandOptionType.SingleValue)]
    public string Audience { get; init; }

    [Option("-i|--issuer", "Token issuer", CommandOptionType.SingleValue)]
    public string Issuer { get; init; }

    [Option("-tt|--token-type", "Token type", CommandOptionType.SingleValue)]
    public string TokenType { get; init; } = "jwt";

    [Option("-f|--file", "Parameters file", CommandOptionType.SingleValue)]
    public string ParametersFile { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        ConsoleService.RenderStatus(() =>
        {
            var parameters = BuildCertificateParameters();
            var isValid = _securityService.ValidateJwtToken(Token, parameters);
            ConsoleService.RenderJwtToken(Token, parameters, isValid);
        });
    }

    private SecurityParameters BuildCertificateParameters()
    {
        if (string.IsNullOrWhiteSpace(ParametersFile))
        {
            return new SecurityParameters
            {
                Certificate = Certificate,
                TokenType = TokenType,
                Password = Password,
                Audience = Audience,
                Issuer = Issuer
            };
        }

        return SecurityParameters.BuildFromFile(ParametersFile);
    }
}
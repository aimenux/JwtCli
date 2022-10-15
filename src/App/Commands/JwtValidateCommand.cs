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
    [Option("-t|--Token", "Jwt token", CommandOptionType.SingleValue)]
    public string Token { get; set; }

    [Option("-c|--certificate", "Certificate file", CommandOptionType.SingleValue)]
    public string Certificate { get; set; }

    [Option("-p|--password", "Certificate password", CommandOptionType.SingleValue)]
    public string Password { get; set; }

    [Option("-a|--audience", "Token audience", CommandOptionType.SingleValue)]
    public string Audience { get; set; }

    [Option("-i|--issuer", "Token issuer", CommandOptionType.SingleValue)]
    public string Issuer { get; set; }

    [Option("-f|--file", "Parameters file", CommandOptionType.SingleValue)]
    public string ParametersFile { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = BuildCertificateParameters();
        var isValid = _securityService.ValidateJwtToken(Token, parameters);
        ConsoleService.RenderJwtToken(Token, parameters, isValid);
    }

    protected override bool HasValidOptions()
    {
        if (string.IsNullOrWhiteSpace(Token)) return false;

        if (string.IsNullOrWhiteSpace(ParametersFile))
        {
            return File.Exists(Certificate) && !string.IsNullOrWhiteSpace(Password);
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
                Audience = Audience,
                Issuer = Issuer
            };
        }

        return SecurityParameters.BuildFromFile(ParametersFile);
    }
}
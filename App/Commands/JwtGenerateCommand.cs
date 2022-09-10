using System.ComponentModel.DataAnnotations;
using App.Services.Certificate;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

[Command(Name = "Generate", FullName = "Generate JWT", Description = "Generate JWT.")]
[VersionOptionFromMember(MemberName = nameof(GetVersion))]
[HelpOption]
public class JwtGenerateCommand : AbstractCommand
{
    private readonly ICertificateService _certificateService;

    public JwtGenerateCommand(ICertificateService certificateService, IConsoleService consoleService) : base(consoleService)
    {
        _certificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
    }

    [Option("-c|--certificate", "Certificate file", CommandOptionType.SingleValue)]
    public string Certificate { get; set; }

    [Option("-p|--password", "Certificate password", CommandOptionType.SingleValue)]
    public string Password { get; set; }

    [Option("-a|--audience", "Token audience", CommandOptionType.SingleValue)]
    public string Audience { get; set; }

    [Option("-i|--issuer", "Token issuer", CommandOptionType.SingleValue)]
    public string Issuer { get; set; }

    [Option("-k|--kid", "Token kid", CommandOptionType.SingleValue)]
    public string Kid { get; set; }

    [Option("-tt|--token-type", "Token type", CommandOptionType.SingleValue)]
    public string TokenType { get; set; } = "jwt";

    [Range(1, int.MaxValue)]
    [Option("-e|--expire-after", "Token expiration in minutes", CommandOptionType.SingleValue)]
    public int ExpireInMinutes { get; set; } = 5;

    [Option("-f|--file", "Parameters file", CommandOptionType.SingleValue)]
    public string ParametersFile { get; set; }

    protected override void Execute(CommandLineApplication app)
    {
        var parameters = new CertificateParameters
        {
            Certificate = Certificate,
            Password = Password,
            ExpireInMinutes = ExpireInMinutes,
            TokenType = TokenType,
            Audience = Audience,
            Issuer = Issuer,
            Kid = Kid
        };

        var token = _certificateService.GenerateJwtToken(parameters);
        ConsoleService.RenderJwtToken(token, parameters);
        ConsoleService.CopyTextToClipboard(token);
    }

    protected override bool HasValidOptions()
    {
        if (!string.IsNullOrWhiteSpace(ParametersFile))
        {
            return File.Exists(ParametersFile);
        }

        return File.Exists(Certificate)
               && !string.IsNullOrWhiteSpace(TokenType)
               && !string.IsNullOrWhiteSpace(Password);
    }

    protected static string GetVersion() => GetVersion(typeof(JwtGenerateCommand));
}
using App.Commands;
using App.Configuration;
using App.Services.Security;
using App.Services.Security.Strategies;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;

namespace Tests.Commands;

public class JwtGenerateCommandTests
{
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "4-tests")]
    [InlineData(@"./Files/ECC.pfx", "4-tests")]
    public void JwtGenerateCommand_Should_Generate_Jwt_Token_Be_Ok(string certificate, string password)
    {
        // arrange
        var app = new CommandLineApplication();
        var strategies = new List<ICertificateStrategy>
        {
            new RsaCertificateStrategy(),
            new EccCertificateStrategy()
        };
        var consoleService = new FakeConsoleService();
        var securityService = new SecurityService(strategies);
        var command = new JwtGenerateCommand(securityService, consoleService)
        {
            Certificate = certificate,
            Password = password
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "")]
    [InlineData(@"./Files/ECC.pfx", "")]
    [InlineData(@"./Files/RSA.pfx", null)]
    [InlineData(@"./Files/ECC.pfx", null)]
    [InlineData(@"./Files/RSA.pfx", "pass")]
    [InlineData(@"./Files/ECC.pfx", "pass")]
    public void JwtGenerateCommand_Should_Generate_Jwt_Token_Be_Ko(string certificate, string password)
    {
        // arrange
        var app = new CommandLineApplication();
        var strategies = new List<ICertificateStrategy>
        {
            new RsaCertificateStrategy(),
            new EccCertificateStrategy()
        };
        var consoleService = new FakeConsoleService();
        var securityService = new SecurityService(strategies);
        var command = new JwtGenerateCommand(securityService, consoleService)
        {
            Certificate = certificate,
            Password = password
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}
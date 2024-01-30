using App.Commands;
using App.Configuration;
using App.Services.Security;
using App.Services.Security.Strategies;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;
using Tests.Helpers;

namespace Tests.Commands;

public class JwtDecodeCommandTests
{
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "4-tests")]
    [InlineData(@"./Files/ECC.pfx", "4-tests")]
    public void JwtDecodeCommand_Should_Decode_Jwt_Token_Be_Ok(string certificate, string password)
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
        var token = securityService.GetToken(certificate, password);
        var command = new JwtDecodeCommand(securityService, consoleService)
        {
            Token = token
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Fact]
    public void JwtDecodeCommand_Should_Decode_Jwt_Token_Be_Ko()
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
        var command = new JwtDecodeCommand(securityService, consoleService)
        {
            Token = Guid.NewGuid().ToString()
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}
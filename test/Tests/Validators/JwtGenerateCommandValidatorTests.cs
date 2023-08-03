using App.Commands;
using App.Services.Console;
using App.Services.Security;
using App.Validators;
using FluentAssertions;
using NSubstitute;

namespace Tests.Validators;

public class JwtGenerateCommandValidatorTests
{
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "pass")]
    [InlineData(@"./Files/ECC.pfx", "pass")]
    public void JwtGenerateCommand_Should_Be_Valid(string certificate, string password)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtGenerateCommand(securityService, consoleService)
        {
            Certificate = certificate,
            Password = password,
            ExpireInMinutes = 5,
            TokenType = "jwt"
        };
        var validator = new JwtGenerateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "", 1)]
    [InlineData(@"./Files/RSA.pfx", "pass", 0)]
    [InlineData(@"./Files/RSA.pfx", "pass", -1)]
    [InlineData(@"./Files/FOOBAR.pfx", "pass", 1)]
    public void JwtGenerateCommand_Should_Not_Be_Valid(string certificate, string password, int expireInMinutes)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtGenerateCommand(securityService, consoleService)
        {
            Certificate = certificate,
            Password = password,
            ExpireInMinutes = expireInMinutes
        };
        var validator = new JwtGenerateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}
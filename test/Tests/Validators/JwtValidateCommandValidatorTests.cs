using App.Commands;
using App.Services.Console;
using App.Services.Security;
using App.Validators;
using FluentAssertions;
using NSubstitute;

namespace Tests.Validators;

public class JwtValidateCommandValidatorTests
{
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "pass", "abc")]
    [InlineData(@"./Files/ECC.pfx", "pass", "abc")]
    public void JwtValidateCommand_Should_Be_Valid(string certificate, string password, string token)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtValidateCommand(securityService, consoleService)
        {
            Certificate = certificate,
            Password = password,
            Token = token
        };
        var validator = new JwtValidateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(@"./Files/RSA.pfx", "", "abc")]
    [InlineData(@"./Files/RSA.pfx", "pass", "")]
    [InlineData(@"./Files/RSA.pfx", "pass", " ")]
    [InlineData(@"./Files/RSA.pfx", "pass", null)]
    [InlineData(@"./Files/FOOBAR.pfx", "pass", "abc")]
    public void JwtValidateCommand_Should_Not_Be_Valid(string certificate, string password, string token)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtValidateCommand(securityService, consoleService)
        {
            Certificate = certificate,
            Password = password,
            Token = token
        };
        var validator = new JwtValidateCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}
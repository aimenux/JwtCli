using App.Commands;
using App.Services.Console;
using App.Services.Security;
using App.Validators;
using FluentAssertions;
using NSubstitute;

namespace Tests.Validators;

public class JwtDecodeCommandValidatorTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    public void JwtDecodeCommand_Should_Be_Valid(string token)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtDecodeCommand(securityService, consoleService)
        {
            Token = token
        };
        var validator = new JwtDecodeCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void JwtDecodeCommand_Should_Not_Be_Valid(string token)
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtDecodeCommand(securityService, consoleService)
        {
            Token = token
        };
        var validator = new JwtDecodeCommandValidator();

        // act
        var result = validator.Validate(command);

        // assert
        result.IsValid.Should().BeFalse();
    }
}
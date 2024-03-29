﻿using App.Commands;
using App.Exceptions;
using App.Services.Console;
using App.Services.Security;
using App.Validators;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;

namespace Tests.Validators;

public class ToolCommandValidatorTests
{
    [Fact]
    public void Should_Get_ValidationErrors_For_ToolCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var command = new ToolCommand(consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_JwtDecodeCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtDecodeCommand(securityService, consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Get_ValidationErrors_For_JwtGenerateCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtGenerateCommand(securityService, consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
        
    [Fact]
    public void Should_Get_ValidationErrors_For_JwtValidateCommand()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var securityService = Substitute.For<ISecurityService>();
        var command = new JwtValidateCommand(securityService, consoleService);

        // act
        var validationErrors = ToolCommandValidator.Validate(command);

        // assert
        validationErrors.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_Throw_Exception_For_Unexpected_Commands()
    {
        // arrange
        var consoleService = Substitute.For<IConsoleService>();
        var command = new UnexpectedCommand(consoleService);

        // act
        var validateFunc = () => ToolCommandValidator.Validate(command);

        // assert
        validateFunc.Should().Throw<JwtCliException>();
    }

    private class UnexpectedCommand : AbstractCommand
    {
        public UnexpectedCommand(IConsoleService consoleService) : base(consoleService)
        {
        }

        protected override void Execute(CommandLineApplication app)
        {
        }
    }
}
using App.Commands;
using App.Configuration;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;

namespace Tests.Commands;

public class ToolCommandTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void Should_ToolCommand_Return_Ok(bool showSettings, bool showVersion)
    {
        // arrange
        var app = new CommandLineApplication();
        var consoleService = new FakeConsoleService();
        var command = new ToolCommand(consoleService)
        {
            ShowSettings = showSettings,
            ShowVersion = showVersion
        };

        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
}
using App.Configuration;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Tests.Commands.Fakes;

namespace Tests.Commands;

public class AbstractCommandTests
{
    [Fact]
    public void Should_Return_Ok()
    {
        // arrange
        var app = new CommandLineApplication();
        var service = new FakeConsoleService();
        var command = new FakeCommand(service)
        {
            Job = () => {}
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ok);
    }
    
    [Fact]
    public void Should_Return_Ko()
    {
        // arrange
        var app = new CommandLineApplication();
        var service = new FakeConsoleService();
        var command = new FakeCommand(service)
        {
            Job = () => throw new Exception("some error has occurred")
        };
        
        // act
        var result = command.OnExecute(app);

        // assert
        result.Should().Be(Settings.ExitCode.Ko);
    }
}
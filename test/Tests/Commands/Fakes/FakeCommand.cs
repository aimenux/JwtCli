using App.Commands;
using App.Services.Console;
using App.Validators;
using McMaster.Extensions.CommandLineUtils;

namespace Tests.Commands.Fakes;

public class FakeCommand : AbstractCommand
{
    public Action Job { get; init; }

    public FakeCommand(IConsoleService consoleService) : base(consoleService)
    {
    }

    protected override void Execute(CommandLineApplication app)
    {
        Job.Invoke();
    }

    protected override bool HasValidOptionsAndArguments(out ValidationErrors validationErrors)
    {
        validationErrors = ValidationErrors.New<FakeCommand>();
        return true;
    }
}
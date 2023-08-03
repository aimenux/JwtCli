using App.Configuration;
using App.Services.Console;
using App.Validators;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

public abstract class AbstractCommand
{
    protected readonly IConsoleService ConsoleService;

    protected AbstractCommand(IConsoleService consoleService)
    {
        ConsoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
    }
    
    public int OnExecute(CommandLineApplication app)
    {
        try
        {
            if (!HasValidOptionsAndArguments(out var validationErrors))
            {
                ConsoleService.RenderValidationErrors(validationErrors);
                return Settings.ExitCode.Ko;
            }

            Execute(app);
            return Settings.ExitCode.Ok;
        }
        catch (Exception ex)
        {
            ConsoleService.RenderException(ex);
            return Settings.ExitCode.Ko;
        }
    }

    protected abstract void Execute(CommandLineApplication app);

    protected virtual bool HasValidOptionsAndArguments(out ValidationErrors validationErrors)
    {
        validationErrors = ToolCommandValidator.Validate(this);
        return !validationErrors.Any();
    }
}
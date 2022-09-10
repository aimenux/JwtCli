using System.Reflection;
using App.Services.Console;
using McMaster.Extensions.CommandLineUtils;

namespace App.Commands;

public abstract class AbstractCommand
{
    protected IConsoleService ConsoleService;

    protected AbstractCommand(IConsoleService consoleService)
    {
        ConsoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
    }

    public void OnExecute(CommandLineApplication app)
    {
        try
        {
            if (!HasValidOptions() || !HasValidArguments())
            {
                throw new Exception($"Invalid options/arguments for command {GetType().Name}");
            }

            Execute(app);
        }
        catch (Exception ex)
        {
            ConsoleService.RenderException(ex);
        }
    }

    protected abstract void Execute(CommandLineApplication app);

    protected virtual bool HasValidOptions() => true;

    protected virtual bool HasValidArguments() => true;

    protected static string GetVersion(Type type)
    {
        return type
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion!;
    }
}
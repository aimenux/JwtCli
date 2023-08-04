using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using App.Configuration;
using App.Extensions;
using App.Services.Security;
using App.Validators;
using Spectre.Console;
using TextCopy;

namespace App.Services.Console;

[ExcludeFromCodeCoverage]
public class ConsoleService : IConsoleService
{
    public ConsoleService()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
    }

    public void RenderTitle(string text)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new FigletText(text));
        AnsiConsole.WriteLine();
    }

    public void RenderVersion(string version)
    {
        var text = $"{Settings.Cli.ToolName} V{version}";
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Markup($"[bold {Color.White}]{text}[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }

    public void CopyTextToClipboard(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            ClipboardService.SetText(text);
        }
    }

    public void RenderSettingsFile(string filepath)
    {
        var name = Path.GetFileName(filepath);
        var json = File.ReadAllText(filepath);
        var formattedJson = json.JsonPrettify();
        var header = new Rule($"[yellow]({name})[/]");
        header.Centered();
        var footer = new Rule($"[yellow]({filepath})[/]");
        footer.Centered();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine(formattedJson);
        AnsiConsole.Write(footer);
        AnsiConsole.WriteLine();
    }

    public void RenderUserSecretsFile(string filepath)
    {
        if (!OperatingSystem.IsWindows()) return;
        if (!File.Exists(filepath)) return;
        if (!GetYesOrNoAnswer("display user secrets", false)) return;
        RenderSettingsFile(filepath);
    }

    public void RenderException(Exception exception)
    {
        const ExceptionFormats formats = ExceptionFormats.ShortenTypes
                                         | ExceptionFormats.ShortenPaths
                                         | ExceptionFormats.ShortenMethods;

        AnsiConsole.WriteLine();
        AnsiConsole.WriteException(exception, formats);
        AnsiConsole.WriteLine();
    }

    public void RenderStatus(Action action)
    {
        var spinner = RandomSpinner();

        AnsiConsole.Status()
            .Start("Work is in progress ...", ctx =>
            {
                ctx.Spinner(spinner);
                action.Invoke();
            });
    }

    public bool GetYesOrNoAnswer(string text, bool defaultAnswer)
    {
        if (AnsiConsole.Confirm($"Do you want to [u]{text}[/] ?", defaultAnswer)) return true;
        AnsiConsole.WriteLine();
        return false;
    }

    public void RenderValidationErrors(ValidationErrors validationErrors)
    {
        var count = validationErrors.Count;

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .Title($"[red][bold]{count} error(s)[/][/]")
            .AddColumn(new TableColumn("[u]Name[/]").Centered())
            .AddColumn(new TableColumn("[u]Message[/]").Centered())
            .Caption("[grey][bold]Invalid options/arguments[/][/]");

        foreach (var error in validationErrors)
        {
            var failure = error.Failure;
            var name = $"[bold]{error.OptionName()}[/]";
            var reason = $"[tan]{failure.ErrorMessage}[/]";
            table.AddRow(ToMarkup(name), ToMarkup(reason));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void RenderSecurityDetails(SecurityDetails securityDetails)
    {
        var subTableColor = Color.LightYellow3;

        var headerTable = new Table()
            .BorderColor(subTableColor)
            .Border(TableBorder.Rounded);

        foreach (var key in securityDetails.Header.Keys)
        {
            headerTable.AddColumn(new TableColumn($"[u]{key}[/]").Centered());
        }

        headerTable.AddRow(securityDetails.Header.Values.ToArray());

        var payloadTable = new Table()
            .BorderColor(subTableColor)
            .Border(TableBorder.Rounded);

        foreach (var key in securityDetails.Payload.Keys)
        {
            payloadTable.AddColumn(new TableColumn($"[u]{key}[/]").Centered());
        }

        payloadTable.AddRow(securityDetails.Payload.Values.ToArray());

        var validFrom = securityDetails.ValidFrom.ToString("F");
        var validTo = securityDetails.ValidTo.ToString("F");

        var validityTable = new Table()
            .BorderColor(subTableColor)
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn($"[u]ValidFrom[/]").Centered())
            .AddColumn(new TableColumn($"[u]ValidTo[/]").Centered())
            .AddRow(validFrom, validTo);

        var table = new Table()
            .BorderColor(Color.White)
            .Border(TableBorder.Square)
            .AddColumn(new TableColumn($"[u][{subTableColor}]Header[/][/]").Centered())
            .AddColumn(new TableColumn($"[u][{subTableColor}]Payload[/][/]").Centered())
            .AddColumn(new TableColumn($"[u][{subTableColor}]Dates[/][/]").Centered());

        table.AddRow(headerTable, payloadTable, validityTable);

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void RenderJwtToken(string token, SecurityParameters parameters)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Markup($"[yellow]{token.EscapeMarkup()}[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Markup("[bold black on yellow]The token is already copied to clipboard.[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }

    public void RenderJwtToken(string token, SecurityParameters parameters, bool isValid)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(isValid ? new Markup("[green]Token is valid[/]") : new Markup("[red]Token is not valid[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }
    
    private static Spinner RandomSpinner() 
    {
        var values = typeof(Spinner.Known)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.PropertyType == typeof(Spinner))
            .Select(x => (Spinner)x.GetValue(null))
            .ToArray();
        var index = Random.Shared.Next(values.Length);
        var value = (Spinner)values.GetValue(index);
        return value;
    }
    
    private static Markup ToMarkup(string text)
    {
        try
        {
            return new Markup(text ?? string.Empty);
        }
        catch
        {
            return ErrorMarkup;
        }
    }

    private static readonly Markup ErrorMarkup = new(Emoji.Known.CrossMark);
}
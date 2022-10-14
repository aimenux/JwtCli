using System.Text;
using App.Extensions;
using App.Services.Certificate;
using Spectre.Console;
using TextCopy;

namespace App.Services.Console;

public class ConsoleService : IConsoleService
{
    public ConsoleService()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
    }

    public void RenderTitle(string text)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new FigletText(text).LeftAligned());
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

    public void RenderException(Exception exception)
    {
        const ExceptionFormats formats = ExceptionFormats.ShortenTypes
                                         | ExceptionFormats.ShortenPaths
                                         | ExceptionFormats.ShortenMethods;

        AnsiConsole.WriteLine();
        AnsiConsole.WriteException(exception, formats);
        AnsiConsole.WriteLine();
    }

    public void RenderJwtToken(string token, CertificateParameters parameters)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Markup($"[yellow]{token.EscapeMarkup()}[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }

    public void RenderJwtToken(string token, CertificateParameters parameters, bool isValid)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(isValid ? new Markup("[green]Token is valid[/]") : new Markup("[red]Token is not valid[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }
}
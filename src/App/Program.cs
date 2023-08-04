using System.Diagnostics.CodeAnalysis;
using App.Commands;
using App.Extensions;
using App.Services.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            await CreateHostBuilder(args).RunCommandLineApplicationAsync<ToolCommand>(args);
        }
        catch (Exception ex)
        {
            var consoleHelper = new ConsoleService();
            consoleHelper.RenderException(ex);
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddUserSecrets();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddNonGenericLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((_, services) =>
            {
                services.Scan(scan =>
                {
                    scan.FromCallingAssembly()
                        .FromAssemblies(typeof(Program).Assembly)
                        .AddClasses()
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });
            });
}
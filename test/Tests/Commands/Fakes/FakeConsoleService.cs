using App.Services.Console;
using App.Services.Security;
using App.Validators;

namespace Tests.Commands.Fakes;

public class FakeConsoleService : IConsoleService
{
    public void RenderTitle(string text)
    {
    }

    public void RenderVersion(string version)
    {
    }

    public void CopyTextToClipboard(string text)
    {
    }

    public void RenderSettingsFile(string filepath)
    {
    }

    public void RenderUserSecretsFile(string filepath)
    {
    }

    public void RenderException(Exception exception)
    {
    }

    public void RenderStatus(Action action)
    {
        action?.Invoke();
    }

    public bool GetYesOrNoAnswer(string text, bool defaultAnswer)
    {
        return defaultAnswer;
    }

    public void RenderValidationErrors(ValidationErrors validationErrors)
    {
    }

    public void RenderSecurityDetails(SecurityDetails securityDetails)
    {
    }

    public void RenderJwtToken(string token, SecurityParameters parameters)
    {
    }

    public void RenderJwtToken(string token, SecurityParameters parameters, bool isValid)
    {
    }
}
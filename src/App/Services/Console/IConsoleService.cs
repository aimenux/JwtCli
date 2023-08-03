using App.Services.Security;
using App.Validators;

namespace App.Services.Console
{
    public interface IConsoleService
    {
        void RenderTitle(string text);
        void RenderVersion(string version);
        void CopyTextToClipboard(string text);
        void RenderSettingsFile(string filepath);
        void RenderUserSecretsFile(string filepath);
        void RenderException(Exception exception);
        void RenderStatus(Action action);
        bool GetYesOrNoAnswer(string text, bool defaultAnswer);
        void RenderValidationErrors(ValidationErrors validationErrors);
        void RenderSecurityDetails(SecurityDetails securityDetails);
        void RenderJwtToken(string token, SecurityParameters parameters);
        void RenderJwtToken(string token, SecurityParameters parameters, bool isValid);
    }
}

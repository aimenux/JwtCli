using App.Services.Security;

namespace App.Services.Console
{
    public interface IConsoleService
    {
        void RenderTitle(string text);
        void CopyTextToClipboard(string text);
        void RenderSettingsFile(string filepath);
        void RenderException(Exception exception);
        void RenderSecurityDetails(SecurityDetails securityDetails);
        void RenderJwtToken(string token, SecurityParameters parameters);
        void RenderJwtToken(string token, SecurityParameters parameters, bool isValid);
    }
}

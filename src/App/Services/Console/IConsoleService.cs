using App.Services.Certificate;

namespace App.Services.Console
{
    public interface IConsoleService
    {
        void RenderTitle(string text);
        void CopyTextToClipboard(string text);
        void RenderSettingsFile(string filepath);
        void RenderException(Exception exception);
        void RenderJwtToken(string token, CertificateParameters parameters);
        void RenderJwtToken(string token, CertificateParameters parameters, bool isValid);
    }
}

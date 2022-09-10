namespace App.Services.Certificate;

public interface ICertificateService
{
    string GenerateJwtToken(CertificateParameters parameters);

    bool ValidateJwtToken(string token, CertificateParameters parameters);
}
namespace App.Services.Certificate.Strategies;

public interface ICertificateStrategy
{
    bool IsMatching(CertificateParameters parameters);

    string CreateJwtToken(CertificateParameters parameters);

    bool VerifyJwtToken(string token, CertificateParameters parameters);
}
namespace App.Services.Security.Strategies;

public interface ICertificateStrategy
{
    bool IsMatching(SecurityParameters parameters);

    string CreateJwtToken(SecurityParameters parameters);

    bool VerifyJwtToken(string token, SecurityParameters parameters);
}
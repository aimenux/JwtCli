namespace App.Services.Security;

public interface ISecurityService
{
    SecurityDetails DecodeJwtToken(string jwt);

    string GenerateJwtToken(SecurityParameters parameters);

    bool ValidateJwtToken(string token, SecurityParameters parameters);
}
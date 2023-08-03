using App.Services.Security;

namespace Tests;

public static class HelperExtensions
{
    public static string GetToken(this ISecurityService securityService, string certificate, string password)
    {
        var token = securityService.GenerateJwtToken(new SecurityParameters
        {
            Certificate = certificate,
            Password = password,
            ExpireInMinutes = 5
        });
        return token;
    }
}
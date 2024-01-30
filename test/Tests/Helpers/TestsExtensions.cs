using App.Services.Security;

namespace Tests.Helpers;

public static class TestsExtensions
{
    public static string GetToken(this ISecurityService securityService, string certificate, string password)
    {
        var token = securityService.GenerateJwtToken(new SecurityParameters
        {
            Certificate = certificate,
            Password = password,
            ExpireInMinutes = 5,
            Kid = Guid.NewGuid().ToString("N")
        });
        return token;
    }
}
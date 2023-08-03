using System.Text.Json;

namespace App.Services.Security;

public class SecurityParameters
{
    public SecurityParameters()
    {
    }

    public SecurityParameters(SecurityParameters parameters)
    {
        Certificate = parameters.Certificate;
        Password = parameters.Password;
        Audience = parameters.Audience;
        Issuer = parameters.Issuer;
        Kid = parameters.Kid;
        TokenType = parameters.TokenType;
        ExpireInMinutes = parameters.ExpireInMinutes;
    }

    public string Certificate { get; init; }
    public string Password { get; init; }
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public string Kid { get; init; }
    public string TokenType { get; init; }
    public int ExpireInMinutes { get; init; }

    public static SecurityParameters BuildFromFile(string parametersFile)
    {
        var json = File.ReadAllText(parametersFile);
        return JsonSerializer.Deserialize<SecurityParameters>(json);
    }
}
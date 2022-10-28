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

    public string Certificate { get; set; }
    public string Password { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Kid { get; set; }
    public string TokenType { get; set; }
    public int ExpireInMinutes { get; set; }

    public static SecurityParameters BuildFromFile(string parametersFile)
    {
        var json = File.ReadAllText(parametersFile);
        return JsonSerializer.Deserialize<SecurityParameters>(json);
    }
}
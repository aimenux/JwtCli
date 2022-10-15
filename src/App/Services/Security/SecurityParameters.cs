using System.Text.Json;

namespace App.Services.Security;

public class SecurityParameters
{
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
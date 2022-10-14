using System.Text.Json;

namespace App.Services.Certificate;

public class CertificateParameters
{
    public string Certificate { get; set; }
    public string Password { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Kid { get; set; }
    public string TokenType { get; set; }
    public int ExpireInMinutes { get; set; }

    public static CertificateParameters BuildFromFile(string parametersFile)
    {
        var json = File.ReadAllText(parametersFile);
        return JsonSerializer.Deserialize<CertificateParameters>(json);
    }
}
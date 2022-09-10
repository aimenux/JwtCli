using App.Services.Certificate.Strategies;

namespace App.Services.Certificate;

public class CertificateService : ICertificateService
{
    private readonly IEnumerable<ICertificateStrategy> _strategies;

    public CertificateService(IEnumerable<ICertificateStrategy> strategies)
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    public string GenerateJwtToken(CertificateParameters parameters)
    {
        var matchingStrategy = _strategies.SingleOrDefault(x => x.IsMatching(parameters));
        if (matchingStrategy is null)
        {
            throw new ArgumentException("Certificate algorithm is not supported !");
        }

        var token = matchingStrategy.CreateJwtToken(parameters);
        return token;
    }

    public bool ValidateJwtToken(string token, CertificateParameters parameters)
    {
        var matchingStrategy = _strategies.SingleOrDefault(x => x.IsMatching(parameters));
        if (matchingStrategy is null)
        {
            throw new ArgumentException("Certificate algorithm is not supported !");
        }

        return matchingStrategy.VerifyJwtToken(token, parameters);
    }
}
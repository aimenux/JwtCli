using System.IdentityModel.Tokens.Jwt;
using App.Services.Security.Strategies;

namespace App.Services.Security;

public class SecurityService : ISecurityService
{
    private readonly IEnumerable<ICertificateStrategy> _strategies;

    public SecurityService(IEnumerable<ICertificateStrategy> strategies)
    {
        _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
    }

    public SecurityDetails DecodeJwtToken(string jwt)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwt);
        var header = ToDictionary(jwtSecurityToken.Header);
        var payload = ToDictionary(jwtSecurityToken.Payload);
        var audience = jwtSecurityToken.Audiences.FirstOrDefault(x => x != null);
        var issuer = jwtSecurityToken.Issuer;
        var validFrom = jwtSecurityToken.ValidFrom;
        var validTo = jwtSecurityToken.ValidTo;
        return new SecurityDetails
        {
            Header = header,
            Payload = payload,
            Audience = audience,
            Issuer = issuer,
            ValidFrom = validFrom,
            ValidTo = validTo
        };
    }

    public string GenerateJwtToken(SecurityParameters parameters)
    {
        var matchingStrategy = _strategies.SingleOrDefault(x => x.IsMatching(parameters));
        if (matchingStrategy is null)
        {
            throw new ArgumentException($"Unsupported certificate '{parameters.Certificate}' !");
        }

        var token = matchingStrategy.CreateJwtToken(parameters);
        return token;
    }

    public bool ValidateJwtToken(string token, SecurityParameters parameters)
    {
        var matchingStrategy = _strategies.SingleOrDefault(x => x.IsMatching(parameters));
        if (matchingStrategy is null)
        {
            throw new ArgumentException($"Unsupported certificate '{parameters.Certificate}' !");
        }

        return matchingStrategy.VerifyJwtToken(token, parameters);
    }

    private IDictionary<string, string> ToDictionary(JwtHeader header)
    {
        var dic = new Dictionary<string, string>();

        foreach (var (key, value) in header)
        {
            dic.Add(key, value.ToString());
        }

        return dic;
    }

    private IDictionary<string, string> ToDictionary(JwtPayload payload)
    {
        var dic = new Dictionary<string, string>();

        foreach (var (key, value) in payload)
        {
            dic.Add(key, value.ToString());
        }

        return dic;
    }
}
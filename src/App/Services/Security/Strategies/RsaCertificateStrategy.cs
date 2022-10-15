using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using App.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Security.Strategies;

public class RsaCertificateStrategy : ICertificateStrategy
{
    private static readonly CryptoProviderFactory CryptoProviderFactory = new()
    {
        CacheSignatureProviders = false
    };

    public bool IsMatching(SecurityParameters parameters)
    {
        try
        {
            using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
            using var rsaPrivateKey = certificate.GetRSAPrivateKey();
            return rsaPrivateKey != null;
        }
        catch
        {
            return false;
        }
    }

    public string CreateJwtToken(SecurityParameters parameters)
    {
        using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
        using var rsaPrivateKey = certificate.GetRSAPrivateKey();
        var securityKey = new RsaSecurityKey(rsaPrivateKey);
        var securityAlgorithm = GetSecurityAlgorithm(securityKey.KeySize);
        var credentials = new SigningCredentials(securityKey, securityAlgorithm)
        {
            CryptoProviderFactory = CryptoProviderFactory
        };
        var expirationDate = DateTime.UtcNow.AddMinutes(parameters.ExpireInMinutes);
        var std = new SecurityTokenDescriptor
        {
            Issuer = parameters.Issuer,
            Audience = parameters.Audience,
            SigningCredentials = credentials,
            TokenType = parameters.TokenType,
            Expires = expirationDate,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(nameof(Environment.UserName), Environment.UserName),
                new Claim(nameof(Environment.MachineName), Environment.MachineName)
            })
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler
            .CreateJwtSecurityToken(std)
            .AddTokenKid(parameters.Kid)
            .AddTokenJti();
        var jws = tokenHandler.WriteToken(token);
        return jws;
    }

    public bool VerifyJwtToken(string token, SecurityParameters parameters)
    {
        using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
        using var rsaPublicKey = certificate.GetRSAPublicKey();
        var securityKey = new RsaSecurityKey(rsaPublicKey);
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
        };
        if (!string.IsNullOrWhiteSpace(parameters.Issuer))
        {
            validationParameters.ValidateIssuer = true;
            validationParameters.ValidIssuer = parameters.Issuer;
        }
        if (!string.IsNullOrWhiteSpace(parameters.Audience))
        {
            validationParameters.ValidateAudience = true;
            validationParameters.ValidAudience = parameters.Audience;
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetSecurityAlgorithm(int keySize)
    {
        return keySize switch
        {
            256 or 2048 => SecurityAlgorithms.RsaSha256,
            384 or 3072 => SecurityAlgorithms.RsaSha384,
            512 or 4096 => SecurityAlgorithms.RsaSha512,
            _ => throw new ArgumentOutOfRangeException(nameof(keySize), $"Unsupported key size '{keySize}' !")
        };
    }
}
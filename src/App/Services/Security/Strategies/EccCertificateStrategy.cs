using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using App.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Security.Strategies;

public class EccCertificateStrategy : ICertificateStrategy
{
    private static readonly CryptoProviderFactory NoCacheFactory = new()
    {
        CacheSignatureProviders = false
    };

    public bool IsMatching(SecurityParameters parameters)
    {
        try
        {
            using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
            using var eccPrivateKey = certificate.GetECDsaPrivateKey();
            return eccPrivateKey != null;
        }
        catch
        {
            return false;
        }
    }

    public string CreateJwtToken(SecurityParameters parameters)
    {
        using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
        using var eccPrivateKey = certificate.GetECDsaPrivateKey();
        var securityKey = new ECDsaSecurityKey(eccPrivateKey);
        var securityAlgorithm = GetSecurityAlgorithm(securityKey.KeySize);
        var credentials = new SigningCredentials(securityKey, securityAlgorithm)
        {
            CryptoProviderFactory = NoCacheFactory
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
                new Claim("user", Environment.UserName),
                new Claim("machine", Environment.MachineName)
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
        using var eccPublicKey = certificate.GetECDsaPublicKey();
        var securityKey = new ECDsaSecurityKey(eccPublicKey);
        var validTypes = string.IsNullOrWhiteSpace(parameters.TokenType)
            ? null
            : new List<string> { parameters.TokenType };
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidTypes = validTypes,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            CryptoProviderFactory = NoCacheFactory,
            ClockSkew = TimeSpan.Zero
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
            256 or 2048 => SecurityAlgorithms.EcdsaSha256,
            384 or 3072 => SecurityAlgorithms.EcdsaSha384,
            512 or 4096 => SecurityAlgorithms.EcdsaSha512,
            _ => throw new ArgumentOutOfRangeException(nameof(keySize), $"Unsupported key size '{keySize}' !")
        };
    }
}
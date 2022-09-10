using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using App.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Certificate.Strategies;

public class EccCertificateStrategy : ICertificateStrategy
{
    public bool IsMatching(CertificateParameters parameters)
    {
        using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
        using var eccPrivateKey = certificate.GetECDsaPrivateKey();
        return eccPrivateKey != null;
    }

    public string CreateJwtToken(CertificateParameters parameters)
    {
        using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
        using var eccPrivateKey = certificate.GetECDsaPrivateKey();
        var securityKey = new ECDsaSecurityKey(eccPrivateKey);
        var securityAlgorithm = GetSecurityAlgorithm(securityKey.KeySize);
        var credentials = new SigningCredentials(securityKey, securityAlgorithm);
        var expirationDate = DateTime.UtcNow.AddMinutes(parameters.ExpireInMinutes);
        var std = new SecurityTokenDescriptor
        {
            Issuer = parameters.Issuer,
            Audience = parameters.Audience,
            SigningCredentials = credentials,
            TokenType = parameters.TokenType,
            Expires = expirationDate
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler
            .CreateJwtSecurityToken(std)
            .AddTokenKid(parameters.Kid)
            .AddTokenJti();
        var jws = tokenHandler.WriteToken(token);
        return jws;
    }

    public bool VerifyJwtToken(string token, CertificateParameters parameters)
    {
        using var certificate = new X509Certificate2(parameters.Certificate, parameters.Password);
        using var eccPrivateKey = certificate.GetECDsaPrivateKey();
        var securityKey = new ECDsaSecurityKey(eccPrivateKey);
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
            tokenHandler.ValidateToken(token, validationParameters, out var _);
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
            _ => throw new ArgumentOutOfRangeException(nameof(keySize), $"Unsupported key size '{keySize}'")
        };
    }
}
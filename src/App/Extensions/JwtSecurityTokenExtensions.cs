using System.IdentityModel.Tokens.Jwt;

namespace App.Extensions;

public static class JwtSecurityTokenExtensions
{
    public static JwtSecurityToken AddTokenJti(this JwtSecurityToken token)
    {
        var jti = Guid.NewGuid().ToString("D");
        if (!token.Payload.TryAdd("jti", jti))
        {
            token.Payload["jti"] = jti;
        }
        return token;
    }

    public static JwtSecurityToken AddTokenKid(this JwtSecurityToken token, string kid)
    {
        if (!token.Header.TryAdd("kid", kid))
        {
            token.Header["kid"] = kid;
        }
        return token;
    }
}
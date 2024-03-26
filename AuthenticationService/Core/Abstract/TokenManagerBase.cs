using Identity.Models;
using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Errors;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Abstract;

public abstract class TokenManagerBase
{
    protected TokenManagerBase()
    {
        Configuration = ConfigurationFactory.GetInstance().Configuration;
        TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:PrivateKey"])),
            ValidAudience = Configuration["JWT:Audience"],
            ValidIssuer = Configuration["JWT:Issuer"],
            RequireExpirationTime = true,
            RequireAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
        };
    }

    protected IConfiguration Configuration { get; }
    protected TokenValidationParameters TokenValidationParameters { get; }

    protected abstract List<Claim> GetClaims(UserEntity userEntity);
    protected abstract SecurityToken GetTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);

    protected virtual string GenerateToken(UserEntity userEntity)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(userEntity);
        var tokenOptions = GetTokenOptions(signingCredentials, claims);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return accessToken;
    }

    protected virtual bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token)) throw TokenError.UnreadableToken;

        var tokenRead = tokenHandler.ReadJwtToken(token);

        var expTime = tokenRead.ValidTo;

        DateTime convertedExpTime = DateTime.SpecifyKind(expTime, DateTimeKind.Utc);
        var kind = expTime.Kind;

        if (DateTime.Now < expTime.ToLocalTime()) return true;

        return false;
    }

    protected virtual ClaimsPrincipal? GetPrincipal(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, TokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    protected virtual SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = Configuration.GetSection("JWT");
        var privateKey = jwtSettings["PrivateKey"];
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }
}
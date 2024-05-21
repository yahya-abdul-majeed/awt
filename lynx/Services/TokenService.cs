using lynx.Models.Auth;
using lynx.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace lynx.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User? user);
        string GenerateRefreshToken();
    }
    public class TokenService : ITokenService
    {
        private readonly JwtOptions options;
        public TokenService(IOptionsMonitor<JwtOptions> options)
        {
            this.options = options.CurrentValue;
        }
        public string GenerateAccessToken(User? user)
        {
            ClaimsIdentity? identity = GetIdentity(user);
            if(identity == null)
            {
                return string.Empty;
            }

            JwtSecurityToken jwt = new(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(30)),
                    signingCredentials: new SigningCredentials(
                            options.GetIssuerSigningKey(),
                            SecurityAlgorithms.HmacSha256
                        )
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private ClaimsIdentity? GetIdentity(User user)
        {
            if (user == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim("name",user.username),
                new Claim("email",user.email),
                new Claim("id",user.id.ToString())
            };

            foreach(var role in user.roles)
            {
                claims.Add(new Claim("roles",role));
            }

            ClaimsIdentity identity = new ClaimsIdentity(claims,"Token");
            return identity;

        }
    }
}

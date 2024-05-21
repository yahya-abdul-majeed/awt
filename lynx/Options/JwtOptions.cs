using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace lynx.Options
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; }  
        public string Audience { get; set; }
        public string Key { get; set; } 
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get;set; }
        public bool ValidateIssuerSigningKey { get;set; }

        public SecurityKey GetIssuerSigningKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}

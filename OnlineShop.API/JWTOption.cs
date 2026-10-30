using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OnlineShop.API
{
    public class JWTOption
    {
        public const string SectionName = "Jwt";
        public string Issuer { get; set; } = "JwtOnlineShope";
        public string Audience { get; set; } = "JwtOnlineShopeApi";

        public string Key { get; set; } = "my-super-secret-key-32-bytes!!";

        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

    }
}

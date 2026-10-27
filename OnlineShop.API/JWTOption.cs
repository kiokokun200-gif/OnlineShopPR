using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OnlineShop.API
{
    public class JWTOption
    {
        public static string SectionName { get; set; } = "JwtDemo";
        public static string Issuer { get; set; } = "JwtOnlineShope";
        public static string Audience { get; set; } = "JwtOnlineShopeApi";

        public static string Key { get; set; } = "my-super-secret-key-32-bytes!!";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

    }
}

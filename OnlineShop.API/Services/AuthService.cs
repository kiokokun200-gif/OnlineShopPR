using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineShop.API.Services
{
    public class AuthService
    {
        private PasswordHasher<User> hasher = new PasswordHasher<User>();

        private JWTOption _jwt = new JWTOption();
        private readonly OnlineShopContext onlineShopContext;
        public AuthService(IOptions<JWTOption> options, OnlineShopContext shopContext)
        {
            _jwt = options.Value;
            onlineShopContext = shopContext;
        }   

        public async Task<(string, bool)> RegisterAsync(string login, string password)
        {
            var usr= await onlineShopContext.Users.FirstOrDefaultAsync(u=>u.Login == login);
            if (usr!=null)
            {
                return ("Существует такой пользователь с таким логином", false);
            }

            User newUser = new User
            {
                Login = login,
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, password);

            await onlineShopContext.Users.AddAsync(newUser);
            return ("Успешно!", true);
        }
        public async Task<(string, bool)> LoginAsync(UserDTO userDTO)
        {
            var user = await onlineShopContext.Users.FirstOrDefaultAsync(u=>u.Login == userDTO.Login);
            
            return ("null", false);
        }

        string CreateToken(UserDTO account)
        {
            var claims = new[]
            {
                new Claim("sub", account.ToString()),
                new Claim("name", account.Login),
                new Claim("role", account.RoleId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(
                    _jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

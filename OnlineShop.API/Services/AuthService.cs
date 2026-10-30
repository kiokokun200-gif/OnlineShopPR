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
                RoleID=1
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, password);

            await onlineShopContext.Users.AddAsync(newUser);
            await onlineShopContext.SaveChangesAsync();
            return ("Успешно!", true);
        }
        public async Task<(string, string?, UserDTO?)> LoginAsync(string login, string password)
        {
            var rolsUser = onlineShopContext.Users.Include(u => u.Role);
            var user = await rolsUser.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
            {
                return ("Такого пользователя нет",null, null);
            }

            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ("Неправильный логин/пароль",null, null);
            }
            var userLogin = new UserDTO
            {
                Login = login,
                RoleName = user.Role.Name,
            };
            string token = CreateToken(userLogin);
            return ("Успешно", token, userLogin );
        }

        string CreateToken(UserDTO account)
        {
            var claims = new[]
            {
                new Claim("sub", account.ToString()),
                new Claim("name", account.Login),
                new Claim("role", account.RoleName)
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

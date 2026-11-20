using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineShop.API.Services
{
    public class AuthService : BaseService
    {
        private PasswordHasher<User> hasher = new PasswordHasher<User>();

        private JWTOption _jwt = new JWTOption();
        public AuthService(IOptions<JWTOption> options, OnlineShopContext shopContext) : base(shopContext)
        {
            _jwt = options.Value;
        }

        public async Task<(string, bool)> RegisterAsync(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return ("Заполните логин и пароль", false);
            }

            var usr = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (usr != null)
            {
                return ("Существует такой пользователь с таким логином", false);
            }

            var roleUser = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Клиент");
            
            if (roleUser == null)
            {
                roleUser = new Role { Name = "Клиент" };
                _context.Roles.Add(roleUser);
                await _context.SaveChangesAsync();
            }

            User newUser = new User
            {
                Login = login,
                RoleId = roleUser.Id
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return ("Успешно!", true);
        }
        public async Task<(string, string?, UserDTO?)> LoginAsync(string login, string password)
        {
            var rolsUser = _context.Users.Include(u => u.Role);
            var user = await rolsUser.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
            {
                return ("Такого пользователя нет", null, null);
            }

            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ("Неправильный логин/пароль", null, null);
            }

            string roleName = user.Role?.Name ?? "Клиент";
            var userLogin = new UserDTO
            {
                UserId = user.UserId,
                Login = user.Login,
                RoleName = roleName,
            };
            string token = CreateToken(userLogin);
            return ("Успешно", token, userLogin);
        }

        string CreateToken(UserDTO account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
                new Claim(ClaimTypes.Name, account.Login),
                new Claim(ClaimTypes.Role, account.RoleName)
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

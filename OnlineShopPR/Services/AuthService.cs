using OnlineShop.Core.DTOs;
using OnlineShop.Core.DTOs.Auth;
using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Xps;

namespace OnlineShopPR.Services
{
    class AuthService
    {
        public static UserDTO? CurrentUser { get; set; }
        public async Task<(bool, string)> LoginAsync(string login, string password)
        {

            var request = new LoginRequest
            {
                login = login,
                password = password
            };


            var response = await ApiService.Client.PostAsJsonAsync("api/auth/login", request);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result == null)
                return (false, "Не удалось получить ответ от сервера");

            if (result.isLogin && !string.IsNullOrEmpty(result.token))
            {
                ApiService.SetToken(result.token);
                CurrentUser = result.user;

                return (true, result.message);
            }

            return (false, result.message);



        }

        public async Task<(bool, string)> RegistrAsync(string login, string password, string confirmPassword)
        {

            var request = new RegisterRequest
            {
                login = login,
                password = password, 
                passwordConfirm = confirmPassword
            };


            var response = await ApiService.Client.PostAsJsonAsync("api/auth/register", request);

            var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();

            if (result == null)
                return (false, "Не удалось получить ответ от сервера");

            

            return (result.isRegister, result.message);



        }
        public void Logout()
        {
            ApiService.ClearToken();
            CurrentUser = null;
        }
    }
}

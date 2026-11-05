using OnlineShop.Core.DTOs;
using OnlineShop.Core.DTOs.Auth;
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
        public async Task<(bool, string)> LoginAsync(string login, string password)
        {

            var request = new LoginRequest
            {
                login = login,
                password = password
            };


            var response = await ApiService.Client.PostAsJsonAsync("api/auth/login", request);

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            
            if (result.isLogin)
            {
                ApiService.SetToken(result.token);
                return (true, result.token);
            }
            else return (false, result.message);
            


        }
    }
}

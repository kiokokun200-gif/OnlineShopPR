using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class ApiService
    {
        public static HttpClient Client = new HttpClient();

        static ApiService()
        {
            Client.BaseAddress =
                new Uri("http://localhost:5000/");
        }

        public static void SetToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}

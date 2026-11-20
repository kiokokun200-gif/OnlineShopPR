using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class KorzinaService
    {
        public async Task<bool> AddProduct(int prodId)
        {
            var res = await ApiService.Client.PostAsync($"api/korzina/addProduct?prodId={prodId}&isAdd=True", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> Remove(int prodId)
        {
            var res = await ApiService.Client.PostAsync($"api/korzina/addProduct?prodId={prodId}&isAdd=false", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<KorzinaProductDTO>> allProductInCart()
        {
            var res = await ApiService.Client.GetFromJsonAsync<IEnumerable<KorzinaProductDTO>>("api/korzina/AllProducts");


            return res;
        }

    }
}

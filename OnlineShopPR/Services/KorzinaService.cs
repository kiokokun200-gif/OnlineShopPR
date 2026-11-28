using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class KorzinaService
    {
        public async Task<(bool, string)> AddProduct(int prodId)
        {
            try
            {
                var res = await ApiService.Client.PostAsync($"api/korzina/addProduct?prodId={prodId}&isAdd=True", null);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                if (obj != null) return (obj.isOk, obj.message);
                return (res.IsSuccessStatusCode, "");
            }
            catch
            {
                return (false, "Ошибка запроса");
            }
        }

        public async Task<(bool, string)> Remove(int prodId)
        {
            try
            {
                var res = await ApiService.Client.PostAsync($"api/korzina/addProduct?prodId={prodId}&isAdd=false", null);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                if (obj != null) return (obj.isOk, obj.message);
                return (res.IsSuccessStatusCode, "");
            }
            catch
            {
                return (false, "Ошибка запроса");
            }
        }

        public async Task<(bool, string)> RemoveProduct(int prodId)
        {
            try
            {
                var res = await ApiService.Client.DeleteAsync($"api/korzina/removeProduct/{prodId}");
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                if (obj != null) return (obj.isOk, obj.message);
                return (res.IsSuccessStatusCode, "");
            }
            catch
            {
                return (false, "Ошибка запроса");
            }
        }

        public async Task<List<KorzinaProductDTO>> allProductInCart()
        {
            var res = await ApiService.Client.GetFromJsonAsync<List<KorzinaProductDTO>>("api/korzina/AllProducts");
            return res ?? [];
        }

    }
}

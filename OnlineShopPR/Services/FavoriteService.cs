using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class FavoriteService
    {
        public async Task<List<ProductDTO>> GetFavorites()
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<ProductDTO>>("api/favorites/All");
                return res ?? new List<ProductDTO>();
            }
            catch { return new List<ProductDTO>(); }
        }

        public async Task<(bool, string)> AddFavorite(int prodId)
        {
            try
            {
                var res = await ApiService.Client.PostAsync($"api/favorites/add/{prodId}", null);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<(bool, string)> RemoveFavorite(int prodId)
        {
            try
            {
                var res = await ApiService.Client.DeleteAsync($"api/favorites/remove/{prodId}");
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<(bool, string)> MoveToCart(int prodId)
        {
            try
            {
                var res = await ApiService.Client.PostAsync($"api/favorites/moveToCart/{prodId}", null);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }
    }
}

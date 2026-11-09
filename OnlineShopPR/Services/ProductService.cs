using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class ProductService
    {
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var res = await ApiService.Client.GetFromJsonAsync<List<ProductDTO>>("api/products/AllProducts");
            return res ?? [];
        }

        public async Task<bool>AddFavorite(int prodID)
        {
            var res = await ApiService.Client.PostAsync($"api/ /{prodID}", null);
            return res.IsSuccessStatusCode;

        }
        public async Task<bool> RemoveFavorite(int prodID)
        {
            var res = await ApiService.Client.DeleteAsync($"api/ /{prodID}");
            return res.IsSuccessStatusCode;

        }
    }
}

using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class ProductService
    {
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<ProductDTO>>("api/products/AllProducts");
                return res ?? [];
            }
            catch { return []; }
        }

        public async Task<ProductDTO?> GetProduct(int id)
        {
            try
            {
                return await ApiService.Client.GetFromJsonAsync<ProductDTO>($"api/products/{id}");
            }
            catch { return null; }
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<CategoryDTO>>("api/products/GetAllCategories");
                return res ?? [];
            }
            catch { return []; }
        }

        public async Task<(bool, string)> AddProduct(ProductDTO product)
        {
            try
            {
                var res = await ApiService.Client.PostAsJsonAsync("api/products/addProduct", product);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<(bool, string)> ChangeProduct(ProductDTO product)
        {
            try
            {
                var req = new HttpRequestMessage(new HttpMethod("PATCH"), "api/products/changeProduct") { Content = JsonContent.Create(product) };
                var res = await ApiService.Client.SendAsync(req);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }
    }

    public class ApiResult
    {
        public bool isOk { get; set; }
        public string message { get; set; } = "";
    }
}

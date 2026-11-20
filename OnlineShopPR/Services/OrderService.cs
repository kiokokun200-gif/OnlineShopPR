using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class OrderService
    {
        public async Task<List<PickupPointDTO>> GetPickupPoints()
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<PickupPointDTO>>("api/orders/pickupPoints");
                return res ?? new List<PickupPointDTO>();
            }
            catch { return new List<PickupPointDTO>(); }
        }

        public async Task<(bool, string)> CreateOrder(CreateOrderRequest req)
        {
            try
            {
                var res = await ApiService.Client.PostAsJsonAsync("api/orders/create", req);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<List<OrderDTO>> GetMyOrders(string sort = "new")
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<OrderDTO>>($"api/orders/my?sort={sort}");
                return res ?? new List<OrderDTO>();
            }
            catch { return new List<OrderDTO>(); }
        }
    }
}

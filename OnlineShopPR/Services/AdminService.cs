using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class AdminService
    {
        public async Task<List<CategoryDTO>> GetCategories()
        {
            try { 
                return await ApiService.Client.GetFromJsonAsync<List<CategoryDTO>>("api/admin/categories") ?? new List<CategoryDTO>(); 
            }
            catch { return new List<CategoryDTO>(); }
        }
        public async Task<(bool, string)> SaveCategory(CategorySaveRequest req)
        {
            try { var res = await ApiService.Client.PostAsJsonAsync("api/admin/categories/save", req); var obj = await res.Content.ReadFromJsonAsync<ApiResult>(); return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? ""); }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<List<PickupPointDTO>> GetPickupPoints()
        {
            try { 
                return await ApiService.Client.GetFromJsonAsync<List<PickupPointDTO>>("api/admin/pickupPoints") ?? new List<PickupPointDTO>(); }
            catch { return new List<PickupPointDTO>(); }
        }
        public async Task<(bool, string)> SavePickupPoint(PickupPointSaveRequest req)
        {
            try { var res = await ApiService.Client.PostAsJsonAsync("api/admin/pickupPoints/save", req); var obj = await res.Content.ReadFromJsonAsync<ApiResult>(); return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? ""); }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<List<StatusDTO>> GetStatuses()
        {
            try { return await ApiService.Client.GetFromJsonAsync<List<StatusDTO>>("api/admin/statuses") ?? new List<StatusDTO>(); }
            catch { return new List<StatusDTO>(); }
        }

        public async Task<List<OrderDTO>> GetOrders()
        {
            try { 
                return await ApiService.Client.GetFromJsonAsync<List<OrderDTO>>("api/admin/orders") ?? new List<OrderDTO>(); 
            }
            catch { return new List<OrderDTO>(); }
        }
        public async Task<(bool, string)> UpdateOrderStatus(OrderStatusUpdateRequest req)
        {
            try { var res = await ApiService.Client.PostAsJsonAsync("api/admin/orders/status", req); var obj = await res.Content.ReadFromJsonAsync<ApiResult>(); return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? ""); }
            catch { return (false, "Ошибка запроса"); }
        }
    }
}

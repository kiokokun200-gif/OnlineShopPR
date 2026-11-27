using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class ReviewService
    {
        public async Task<List<ReviewDTO>> GetReviews(int productId)
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<ReviewDTO>>($"api/reviews/product/{productId}");
                return res ?? new List<ReviewDTO>();
            }
            catch { return new List<ReviewDTO>(); }
        }

        public async Task<List<ReviewDTO>> GetAllReviews()
        {
            try
            {
                var res = await ApiService.Client.GetFromJsonAsync<List<ReviewDTO>>("api/reviews/all");
                return res ?? new List<ReviewDTO>();
            }
            catch { return new List<ReviewDTO>(); }
        }

        public async Task<(bool, string)> AddReview(ReviewCreateRequest req)
        {
            try
            {
                var res = await ApiService.Client.PostAsJsonAsync("api/reviews/add", req);
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }

        public async Task<(bool, string)> DeleteReview(int reviewId)
        {
            try
            {
                var res = await ApiService.Client.DeleteAsync($"api/reviews/delete/{reviewId}");
                var obj = await res.Content.ReadFromJsonAsync<ApiResult>();
                return (obj?.isOk ?? res.IsSuccessStatusCode, obj?.message ?? "");
            }
            catch { return (false, "Ошибка запроса"); }
        }
    }
}

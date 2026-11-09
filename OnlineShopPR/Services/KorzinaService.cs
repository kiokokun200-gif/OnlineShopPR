using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopPR.Services
{
    class KorzinaService
    {
        public async Task<bool> AddProduct(int prodId)
        {
            var res = await ApiService.Client.PostAsync($"api/korzina/addProduct/{prodId}/true", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> Remove(int prodId)
        {
            var res = await ApiService.Client.PostAsync($"api/korzina/addProduct/{prodId}/false", null);
            return res.IsSuccessStatusCode;
        }
    }
}

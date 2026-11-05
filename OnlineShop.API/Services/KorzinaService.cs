using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace OnlineShop.API.Services
{
    public class KorzinaService : BaseService
    {
        public KorzinaService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public async Task<int> AddKorzina(int userId)
        {
            var korzina = new Korzina
            {
                UserID = userId,
                TotalPrice = 0,
            };

            await _context.Korzinas.AddAsync(korzina);
            await _context.SaveChangesAsync();
            return korzina.KorzinaID;
        }


        public async Task<IEnumerable<ProductDTO>> GetAllProducts(int userId)
        {
            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k=>k.UserID == userId);
            if (korzina == null)
            {
                await AddKorzina(userId);
                return [];
            }
            var products = _context.KorzinaProducts.Where(k=>k.KorzinaID == korzina.KorzinaID).Select(p=>p.Product)
                .Include(cat => cat.Categori)
                .Select(p =>
                new ProductDTO
                {
                    PathImg = p.PathImg,
                    CategoriName = p.Categori.Name,
                    Name = p.Name,
                    Description = p.Discription,
                    isActive = p.isActive,
                    IsFavorite = userId == 0 ? false : p.FavoriteProducts.FirstOrDefault(f => f.UserID == userId) != null,
                    Price = p.Price,
                    Rating = p.Rating,
                    QuantityInPack = p.QuantityInPack,
                    QuantityInStock = p.QuantityInStock,
                    ReviewCount = p.ReviewProducts.Count
                }
                );
            return products;
        }

        public async Task<(bool, string)> AddProductToKorzina(int userId, int prodId, bool IsAdd)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p=>p.ProductID == prodId);
            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k => k.UserID == userId);
            var korzinaId = korzina?.KorzinaID ?? -1;
            if (korzinaId == -1)
            {
                korzinaId = await AddKorzina(userId);
            }
            var quantProdInKorz = 0;
            if (korzina is null)
            {
                quantProdInKorz = _context.KorzinaProducts.Where(k => k.KorzinaID == korzinaId && k.ProductID == prodId).Count();
            }
            if (quantProdInKorz > product.QuantityInStock)
            {
                return (false, "");
            }
            var korzinaProd = new KorzinaProduct
            {
                ProductID = product.ProductID,
                KorzinaID = korzinaId,

            };
            return (true, $"Продукт '{product.Name}' успешно добавлен в корзину");
        }





    }
}

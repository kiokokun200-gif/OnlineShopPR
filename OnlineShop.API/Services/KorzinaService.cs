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
                UserId = userId,
                TotalPrice = 0,
            };

            await _context.Korzinas.AddAsync(korzina);
            await _context.SaveChangesAsync();
            return korzina.KorzinaId;
        }


        public async Task<IEnumerable<KorzinaProductDTO>> GetAllProducts(int userId)
        {
            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k=>k.UserId == userId);
            if (korzina == null)
            {
                await AddKorzina(userId);
                return [];
            }
            var korzProd = _context.KorzinaProducts.Where(k => k.KorzinaId == korzina.KorzinaId);
            if (korzProd.Count() == 0) return [];
            var products = korzProd
                //.Include(cat => cat.Categori)
                .Select(p =>
                new KorzinaProductDTO
                {
                    ProductDTO = new ProductDTO
                    {
                        ProductID = p.ProductId,
                        PathImg = p.Product.PathImg,
                        CategoriName = p.Product.Categori.Name,
                        Name = p.Product.Name,
                        Description = p.Product.Discription,
                        isActive = p.Product.IsActive,
                        IsFavorite = userId == 0 ? false : p.Product.FavoriteProducts.FirstOrDefault(f => f.UserId == userId) != null,
                        Price = p.Product.Price,
                        Rating = p.Product.Rating,
                        QuantityInPack = p.Product.QuantityInPack,
                        QuantityInStock = p.Product.QuantityInStock,
                        ReviewCount = p.Product.ReviewProducts.Count
                    },
                    Quantity = p.Quantity

                }
                );



            return products;
        }

        public async Task<(bool, string)> AddProductToKorzina(int userId, int prodId, bool IsAdd)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p=>p.ProductId == prodId);
            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k => k.UserId == userId);
            var korzinaId = korzina?.KorzinaId ?? -1;
            if (korzinaId == -1)
            {
                korzinaId = await AddKorzina(userId);
            }
            bool isNewProd = false;
            var quantProdInKorz = 0;


            KorzinaProduct korzinProd = null;
            if (korzina is null)
            {
                korzinProd = await _context.KorzinaProducts.FirstOrDefaultAsync(k => k.KorzinaId == korzinaId && k.ProductId == prodId);
                if (korzinProd is null) isNewProd = true;
                else
                {
                    quantProdInKorz = korzinProd.Quantity;
                }
            }
            quantProdInKorz += IsAdd ? 1 : -1;

            if (quantProdInKorz > product.QuantityInStock || quantProdInKorz < 0)
            {
                return (false, "");
            }
            else if (quantProdInKorz == 0)
            {

            }
            if (isNewProd)
            {
                var korzinaProd = new KorzinaProduct
                {
                    ProductId = product.ProductId,
                    KorzinaId = korzinaId,
                    Quantity = 1
                };

                await _context.KorzinaProducts.AddAsync(korzinaProd);
                await _context.SaveChangesAsync();
                return (true, $"Продукт '{product.Name}' успешно добавлен в корзину");

            }
            korzinProd.Quantity = quantProdInKorz;
            await _context.SaveChangesAsync();

            return (true, $"");

        }





    }
}

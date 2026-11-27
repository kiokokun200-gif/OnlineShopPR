using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class FavoriteService : BaseService
    {
        public FavoriteService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public async Task<List<ProductDTO>> GetFavorites(int userId)
        {
            var favIds = await _context.FavoriteProducts.Where(f => f.UserId == userId).Select(f => f.ProductId).ToListAsync();
            var list = await _context.Products.Include(p => p.Categori).Include(p => p.ReviewProducts)
                .Where(p => favIds.Contains(p.ProductId))
                .Select(p => new ProductDTO
                {
                    ProductID = p.ProductId,
                    Name = p.Name,
                    Description = p.Discription,
                    CategoriName = p.Categori.Name,
                    CategoriId = p.CategoriId,
                    Price = p.Price,
                    PathImg = p.PathImg,
                    Rating = _context.ReviewProducts.Where(r=>r.ProductId == p.ProductId).Sum(r=>r.Rating) /  Math.Max(1, _context.ReviewProducts.Count(r => r.ProductId == p.ProductId)),
                    QuantityInPack = p.QuantityInPack,
                    QuantityInStock = p.QuantityInStock,
                    isActive = p.IsActive,
                    ReviewCount = p.ReviewProducts.Count,
                    IsFavorite = true,
                    isInCart = _context.KorzinaProducts.Any(kp => kp.ProductId == p.ProductId && kp.Korzina.UserId == userId),
                    quantityInCart = _context.KorzinaProducts.Where(kp => kp.ProductId == p.ProductId && kp.Korzina.UserId == userId).Select(kp => kp.Quantity).FirstOrDefault()
                }).ToListAsync();
            return list;
        }

        public async Task<(bool, string)> AddFavorite(int userId, int productId)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (prod == null) return (false, "Товар не найден");
            var exists = await _context.FavoriteProducts.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (exists != null) return (true, "Уже в избранном");
            _context.FavoriteProducts.Add(new FavoriteProduct { UserId = userId, ProductId = productId });
            await _context.SaveChangesAsync();
            return (true, "Добавлено в избранное");
        }

        public async Task<(bool, string)> RemoveFavorite(int userId, int productId)
        {
            var exists = await _context.FavoriteProducts.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (exists == null) return (true, "Уже удалено");
            _context.FavoriteProducts.Remove(exists);
            await _context.SaveChangesAsync();
            return (true, "Удалено из избранного");
        }

        public async Task<(bool, string)> MoveToCart(int userId, int productId)
        {
            var exists = await _context.FavoriteProducts.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (exists != null)
            {
                _context.FavoriteProducts.Remove(exists);
            }

            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k => k.UserId == userId);
            if (korzina == null)
            {
                korzina = new Korzina { UserId = userId, TotalPrice = 0 };
                _context.Korzinas.Add(korzina);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) return (false, "Товар не найден");
            if (!product.IsActive || product.QuantityInStock <= 0) return (false, "Товар недоступен для покупки");

            var kp = await _context.KorzinaProducts.FirstOrDefaultAsync(k => k.KorzinaId == korzina.KorzinaId && k.ProductId == productId);
            if (kp == null)
            {
                kp = new KorzinaProduct { KorzinaId = korzina.KorzinaId, ProductId = productId, Quantity = 1 };
                _context.KorzinaProducts.Add(kp);
            }
            else
            {
                if (kp.Quantity + 1 > product.QuantityInStock) return (false, "Недостаточно товара на складе");
                kp.Quantity += 1;
            }

            await _context.SaveChangesAsync();
            await RecalcTotal(korzina.KorzinaId);
            return (true, "Товар добавлен в корзину");
        }

        private async Task RecalcTotal(int korzinaId)
        {
            var korzina = await _context.Korzinas.Include(k => k.KorzinaProducts).ThenInclude(kp => kp.Product).FirstOrDefaultAsync(k => k.KorzinaId == korzinaId);
            if (korzina == null) return;
            korzina.TotalPrice = korzina.KorzinaProducts.Sum(k => k.Quantity * k.Product.Price);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class KorzinaService : BaseService
    {
        public KorzinaService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public async Task<Korzina> GetOrCreateKorzina(int userId)
        {
            var korzina = await _context.Korzinas.Include(k => k.KorzinaProducts).FirstOrDefaultAsync(k => k.UserId == userId);
            if (korzina == null)
            {
                korzina = new Korzina { UserId = userId, TotalPrice = 0 };
                _context.Korzinas.Add(korzina);
                await _context.SaveChangesAsync();
            }
            return korzina;
        }

        public async Task<IEnumerable<KorzinaProductDTO>> GetAllProducts(int userId)
        {
            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k => k.UserId == userId);
            if (korzina == null)
            {
                await GetOrCreateKorzina(userId);
                return new List<KorzinaProductDTO>();
            }

            var products = await _context.KorzinaProducts
                .Where(k => k.KorzinaId == korzina.KorzinaId)
                .Include(k => k.Product).ThenInclude(p => p.Categori)
                .Include(k => k.Product).ThenInclude(p => p.ReviewProducts)
                .ToListAsync();

            return products.Select(p => new KorzinaProductDTO
            {
                Quantity = p.Quantity,
                ProductDTO = new ProductDTO
                {
                    ProductID = p.ProductId,
                    PathImg = p.Product.PathImg,
                    CategoriName = p.Product.Categori?.Name ?? "",
                    CategoriId = p.Product.CategoriId,
                    Name = p.Product.Name,
                    Description = p.Product.Discription,
                    isActive = p.Product.IsActive,
                    IsFavorite = _context.FavoriteProducts.Any(f => f.UserId == userId && f.ProductId == p.ProductId),
                    Price = p.Product.Price,
                    Rating = p.Product.Rating,
                    QuantityInPack = p.Product.QuantityInPack,
                    QuantityInStock = p.Product.QuantityInStock,
                    ReviewCount = p.Product.ReviewProducts.Count,
                    isInCart = p.Quantity > 0,
                    quantityInCart = p.Quantity
                }
            }).ToList();
        }

        public async Task<(bool, string)> AddProductToKorzina(int userId, int prodId, bool isAdd)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == prodId);
            if (product == null) return (false, "Товар не найден");
            if (!product.IsActive || product.QuantityInStock <= 0) return (false, "Товар недоступен для покупки");

            var korzina = await GetOrCreateKorzina(userId);
            var korzinProd = await _context.KorzinaProducts.FirstOrDefaultAsync(k => k.KorzinaId == korzina.KorzinaId && k.ProductId == prodId);

            if (korzinProd == null)
            {
                if (!isAdd) return (false, "Товара нет в корзине");
                if (product.QuantityInStock < 1) return (false, "Недостаточно товара на складе");
                korzinProd = new KorzinaProduct
                {
                    ProductId = product.ProductId,
                    KorzinaId = korzina.KorzinaId,
                    Quantity = 1
                };
                await _context.KorzinaProducts.AddAsync(korzinProd);
                await _context.SaveChangesAsync();
                await RecalcTotal(korzina.KorzinaId);
                return (true, $"Товар '{product.Name}' добавлен в корзину");
            }

            int newQuantity = korzinProd.Quantity + (isAdd ? 1 : -1);
            if (newQuantity > product.QuantityInStock) return (false, "Недостаточно товара на складе");

            if (newQuantity <= 0)
            {
                _context.KorzinaProducts.Remove(korzinProd);
                await _context.SaveChangesAsync();
                await RecalcTotal(korzina.KorzinaId);
                return (true, "Товар удален из корзины");
            }

            korzinProd.Quantity = newQuantity;
            await _context.SaveChangesAsync();
            await RecalcTotal(korzina.KorzinaId);
            return (true, "Количество обновлено");
        }

        public async Task<(bool, string)> RemoveProduct(int userId, int prodId)
        {
            var korzina = await _context.Korzinas.FirstOrDefaultAsync(k => k.UserId == userId);
            if (korzina == null) return (false, "Корзина не найдена");
            var korzinProd = await _context.KorzinaProducts.FirstOrDefaultAsync(k => k.KorzinaId == korzina.KorzinaId && k.ProductId == prodId);
            if (korzinProd == null) return (false, "Товар не найден в корзине");
            _context.KorzinaProducts.Remove(korzinProd);
            await _context.SaveChangesAsync();
            await RecalcTotal(korzina.KorzinaId);
            return (true, "Товар удален из корзины");
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

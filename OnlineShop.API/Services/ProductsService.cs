using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class ProductsService
    {
        private readonly OnlineShopContext _context;
        public ProductsService(OnlineShopContext shopContext)
        {
            _context = shopContext;
        }

        public List<ProductDTO> GetAllProducts(int userId)
        {
            var products = _context.Products
                .Include(cat => cat.Categori)
                .Select(p =>
                new ProductDTO
                {
                    PathImg = p.PathImg,
                    CategoriName = p.Categori.Name,
                    Name = p.Name,
                    Discription = p.Discription,
                    isActive = p.isActive,
                    IsFavorite = userId == 0 ? false : p.FavoriteProducts.FirstOrDefault(f => f.UserID == userId) != null,
                    Price = p.Price,
                    Rating = p.Rating,
                    QuantityInPack = p.QuantityInPack,
                    QuantityInStock = p.QuantityInStock,
                    ReviewCount = p.ReviewProducts.Count,


                }
                )
                
                .ToList();
            return products;
        }

    }
}

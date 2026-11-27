using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class ProductsService : BaseService
    {
        public ProductsService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public List<ProductDTO> GetAllProducts(int userId)
        {
            var products = _context.Products
                .Include(cat => cat.Categori)
                .Include(p => p.ReviewProducts)
                .Include(p => p.KorzinaProducts).ThenInclude(kp => kp.Korzina)
                .ToList()
                .Select(p =>
                {
                    var userCartItems = p.KorzinaProducts.Where(kp => kp.Korzina != null && kp.Korzina.UserId == userId).ToList();
                    int qInCart = userCartItems.Sum(k => k.Quantity);
                    return new ProductDTO
                    {
                        ProductID = p.ProductId,
                        PathImg = p.PathImg,
                        CategoriName = p.Categori?.Name ?? "",
                        CategoriId = p.CategoriId,
                        Name = p.Name,
                        Description = p.Discription,
                        isActive = p.IsActive,
                        IsFavorite = userId == 0 ? false : _context.FavoriteProducts.Any(f => f.ProductId == p.ProductId && f.UserId == userId),
                        Price = p.Price,
                        Rating = _context.ReviewProducts.Where(r => r.ProductId == p.ProductId).Sum(r => r.Rating) / Math.Max(1, _context.ReviewProducts.Count(r => r.ProductId == p.ProductId)),
                        QuantityInPack = p.QuantityInPack,
                        QuantityInStock = p.QuantityInStock,
                        ReviewCount = p.ReviewProducts.Count,
                        isInCart = qInCart > 0,
                        quantityInCart = qInCart
                    };
                }).ToList();
            return products;
        }

        public ProductDTO? GetProductById(int id, int userId)
        {
            var p = _context.Products
                .Include(cat => cat.Categori)
                .Include(r => r.ReviewProducts)
                .Include(k => k.KorzinaProducts).ThenInclude(kp => kp.Korzina)
                .FirstOrDefault(x => x.ProductId == id);
            if (p == null) return null;

            var userCartItems = p.KorzinaProducts.Where(kp => kp.Korzina != null && kp.Korzina.UserId == userId).ToList();
            int qInCart = userCartItems.Sum(k => k.Quantity);

            return new ProductDTO
            {
                ProductID = p.ProductId,
                PathImg = p.PathImg,
                CategoriName = p.Categori?.Name ?? "",
                CategoriId = p.CategoriId,
                Name = p.Name,
                Description = p.Discription,
                isActive = p.IsActive,
                IsFavorite = userId == 0 ? false : _context.FavoriteProducts.Any(f => f.ProductId == p.ProductId && f.UserId == userId),
                Price = p.Price,
                Rating = _context.ReviewProducts.Where(r => r.ProductId == p.ProductId).Sum(r => r.Rating) / Math.Max(1, _context.ReviewProducts.Count(r => r.ProductId == p.ProductId)),
                QuantityInPack = p.QuantityInPack,
                QuantityInStock = p.QuantityInStock,
                ReviewCount = p.ReviewProducts.Count,
                isInCart = qInCart > 0,
                quantityInCart = qInCart
            };
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            var categories = _context.Categories.Select(c =>
                new CategoryDTO
                {
                    CategoryID = c.CategoryId,
                    Name = c.Name,
                }
            );
            return categories.ToList();
        }

        public async Task<(bool, string)> AddNewProduct(ProductDTO productNew)
        {
            var findProd = await _context.Products.FirstOrDefaultAsync(p => p.Name == productNew.Name);
            if (findProd != null)
            {
                return (false, "Продукт с таким названием уже есть");
            }

            var cat = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == productNew.CategoriId || c.Name == productNew.CategoriName);
            if (cat == null)
            {
                return (false, "Категория не найдена");
            }

            Product product = new()
            {
                Name = productNew.Name,
                Discription = productNew.Description,
                CategoriId = cat.CategoryId,
                Price = productNew.Price,
                PathImg = string.IsNullOrWhiteSpace(productNew.PathImg) ? null : productNew.PathImg,
                Rating = 0,
                QuantityInPack = productNew.QuantityInPack,
                QuantityInStock = productNew.QuantityInStock,
                IsActive = productNew.isActive
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return (true, $"Продукт '{productNew.Name}' успешно добавлен");
        }

        public async Task<(bool, string)> ChangeProduct(ProductDTO productNewField)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productNewField.ProductID);
            if (product == null)
            {
                return (false, "Такого продута нет");
            }

            var cat = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == productNewField.CategoriId || c.Name == productNewField.CategoriName);
            if (cat == null)
            {
                return (false, "Категория не найдена");
            }

            product.Name = productNewField.Name;
            product.Discription = productNewField.Description;
            product.Price = productNewField.Price;
            product.PathImg = string.IsNullOrWhiteSpace(productNewField.PathImg) ? product.PathImg : productNewField.PathImg;
            product.CategoriId = cat.CategoryId;
            product.QuantityInPack = productNewField.QuantityInPack;
            product.QuantityInStock = productNewField.QuantityInStock;
            product.IsActive = productNewField.isActive;

            await _context.SaveChangesAsync();

            return (true, $"Продукт '{productNewField.Name}' успешно обновлен");
        }
    }
}

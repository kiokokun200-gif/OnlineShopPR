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

        public IEnumerable<ProductDTO> GetAllProducts(int userId)
        {
            var products = _context.Products
                .Include(cat => cat.Categori)
                .Select(p =>
                new ProductDTO
                {
                    PathImg = p.PathImg,
                    CategoriName = p.Categori.Name,
                    Name = p.Name,
                    Description = p.Discription,
                    isActive = p.IsActive,
                    IsFavorite = userId == 0 ? false : p.FavoriteProducts.FirstOrDefault(f => f.UserId == userId) != null,
                    Price = p.Price,
                    Rating = p.Rating,
                    QuantityInPack = p.QuantityInPack,
                    QuantityInStock = p.QuantityInStock,
                    ReviewCount = p.ReviewProducts.Count
                }
                );
            return products;
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
            return categories;
        }


        public async Task<(bool, string)> AddNewProduct(ProductDTO productNew)
        {
            var findProd = await _context.Products.FirstOrDefaultAsync(p => p.Name == productNew.Name);
            if (findProd != null)
            {
                return (false, "Продукт с таким названием уже есть");
            }



            Product product = new()
            {
                Name = productNew.Name,
                Discription = productNew.Description,
                Categori = await _context.Categories.FirstAsync(c => c.Name == productNew.Name),
                Price = productNew.Price,
                PathImg = productNew.PathImg,
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
            var product = await _context.Products.FirstOrDefaultAsync(p=>p.ProductId == productNewField.ProductID);
            if (product == null)
            {
                return (false, "Такого продута нет");
            }
            product.Name = productNewField.Name;
            product.Discription = productNewField.Description;
            product.Price = productNewField.Price;
            product.PathImg = productNewField.PathImg;
            product.Rating = productNewField.Rating ?? 0;
            product.Categori = await _context.Categories.FirstAsync(c => c.Name == productNewField.Name);
            product.QuantityInPack = productNewField.QuantityInPack;
            product.QuantityInStock = productNewField.QuantityInStock;
            product.IsActive = productNewField.isActive;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();


            return (true, $"Продукт '{productNewField.Name}' успешно обновлен");
        }
    }


   
}

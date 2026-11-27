using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class ReviewService : BaseService
    {
        public ReviewService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public async Task<List<ReviewDTO>> GetReviewsByProduct(int productId)
        {
            return await _context.ReviewProducts.Include(r => r.User).Include(r => r.Product)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.ReviewDate)
                .Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    ProductName = r.Product.Name,
                    UserLogin = r.User.Login,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    ReviewDate = r.ReviewDate
                }).ToListAsync();
        }

        public async Task<List<ReviewDTO>> GetAllReviews()
        {
            return await _context.ReviewProducts.Include(r => r.User).Include(r => r.Product)
                .OrderByDescending(r => r.ReviewDate)
                .Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    ProductName = r.Product.Name,
                    UserLogin = r.User.Login,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    ReviewDate = r.ReviewDate
                }).ToListAsync();
        }

        public async Task<(bool, string)> AddReview(int userId, ReviewCreateRequest req)
        {
            if (req.Rating < 1 || req.Rating > 5) return (false, "Оценка должна быть от 1 до 5");
            if (string.IsNullOrWhiteSpace(req.ReviewText)) return (false, "Введите текст отзыва");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == req.ProductId);
            if (product == null) return (false, "Товар не найден");

            var exists = await _context.ReviewProducts.FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == req.ProductId);
            if (exists != null) return (false, "Вы уже оставляли отзыв на этот товар");

            var review = new ReviewProduct
            {
                UserId = userId,
                ProductId = req.ProductId,
                Rating = req.Rating,
                ReviewText = req.ReviewText,
                ReviewDate = DateTime.Now
            };
            _context.ReviewProducts.Add(review);
            await _context.SaveChangesAsync();
            await RecalcRating(req.ProductId);
            return (true, "Отзыв добавлен");
        }

        public async Task<(bool, string)> DeleteReview(int reviewId)
        {
            var review = await _context.ReviewProducts.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null) return (false, "Отзыв не найден");
            int productId = review.ProductId;
            _context.ReviewProducts.Remove(review);
            await _context.SaveChangesAsync();
            await RecalcRating(productId);
            return (true, "Отзыв удален");
        }

        private async Task RecalcRating(int productId)
        {
            var product = await _context.Products.Include(p => p.ReviewProducts).FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) return;
            if (product.ReviewProducts.Count == 0)
                product.Rating = 0;
            else
                product.Rating = (decimal)product.ReviewProducts.Average(r => (double)r.Rating);
            await _context.SaveChangesAsync();
        }
    }
}

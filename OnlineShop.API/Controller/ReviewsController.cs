using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using OnlineShop.Core.DTOs.DB;
using System.Security.Claims;

namespace OnlineShop.API.Controller
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        public ReviewsController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            return Ok(await _reviewService.GetReviewsByProduct(productId));
        }

        [HttpGet("all")]
        [Authorize(Roles = "Админ")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _reviewService.GetAllReviews());
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddReview(ReviewCreateRequest req)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _reviewService.AddReview(userId, req);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpDelete("delete/{reviewId}")]
        [Authorize(Roles = "Админ")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var res = await _reviewService.DeleteReview(reviewId);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }
    }
}

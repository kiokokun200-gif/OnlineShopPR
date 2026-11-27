using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using System.Security.Claims;

namespace OnlineShop.API.Controller
{
    [ApiController]
    [Route("api/favorites")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;
        public FavoritesController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetFavorites()
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            return Ok(await _favoriteService.GetFavorites(userId));
        }

        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddFavorite(int productId)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _favoriteService.AddFavorite(userId, productId);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _favoriteService.RemoveFavorite(userId, productId);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpPost("moveToCart/{productId}")]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _favoriteService.MoveToCart(userId, productId);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using System.Security.Claims;

namespace OnlineShop.API.Controller
{
    [ApiController]
    [Route("api/korzina")]
    [Authorize]
    public class KorzinaController : ControllerBase
    {
        private readonly KorzinaService _korzinaService;
        public KorzinaController(KorzinaService service)
        {
            _korzinaService = service;
        }

        [HttpGet("AllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var products = await _korzinaService.GetAllProducts(userId);
            return Ok(products);
        }


        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProductToKorzina(int prodId, bool isAdd)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _korzinaService.AddProductToKorzina(userId, prodId, isAdd);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpDelete("removeProduct/{prodId}")]
        public async Task<IActionResult> RemoveProduct(int prodId)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _korzinaService.RemoveProduct(userId, prodId);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

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
            Debug.WriteLine(userStringId);
            _ = int.TryParse(userStringId, out int userId);
            var products = await _korzinaService.GetAllProducts(userId);

            return Ok(products);
            //return Ok( await _korzinaService.GetAllProducts(userId));
        }


        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProductToKorzina(int prodId, bool isAdd)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            return Ok( await _korzinaService.AddProductToKorzina(userId, prodId, isAdd));
        }

    }
}

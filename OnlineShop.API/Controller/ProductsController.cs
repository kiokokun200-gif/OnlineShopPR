using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using OnlineShop.Core.DTOs.DB;
using System.Security.Claims;

namespace OnlineShop.API.Controller
{

    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productsService;
        public ProductsController(ProductsService service)
        {
            _productsService = service;
        }


        [HttpGet("AllProducts")]
        public IActionResult GetAllProducts()
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userStringId, out int userId);
            return Ok(_productsService.GetAllProducts(userId));
        }
    }
}

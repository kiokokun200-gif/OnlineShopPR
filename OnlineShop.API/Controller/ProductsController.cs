using Microsoft.AspNetCore.Authorization;
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
            _ = int.TryParse(userStringId, out int userId);
            return Ok(_productsService.GetAllProducts(userId));
        }

        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            return Ok(_productsService.GetAllCategories());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var prod = _productsService.GetProductById(id, userId);
            if (prod == null) return NotFound();
            return Ok(prod);
        }

        [HttpPost("addProduct")]
        [Authorize(Roles = "Админ")]
        public async Task<IActionResult> AddNewProduct(ProductDTO NewProduct)
        {
            var res = await _productsService.AddNewProduct(NewProduct);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpPatch("changeProduct")]
        [Authorize(Roles = "Админ")]
        public async Task<IActionResult> ChangeProduct(ProductDTO product)
        {
            var res = await _productsService.ChangeProduct(product);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using OnlineShop.Core.DTOs.DB;
using System.Security.Claims;

namespace OnlineShop.API.Controller
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("pickupPoints")]
        public async Task<IActionResult> GetPickupPoints()
        {
            return Ok(await _orderService.GetPickupPoints());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest req)
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            var res = await _orderService.CreateOrder(userId, req);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders(string sort = "new")
        {
            string? userStringId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userStringId, out int userId);
            return Ok(await _orderService.GetUserOrders(userId, sort));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Controller
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Админ")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _adminService.GetCategories());
        }

        [HttpPost("categories/save")]
        public async Task<IActionResult> SaveCategory(CategorySaveRequest req)
        {
            var res = await _adminService.SaveCategory(req);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpGet("pickupPoints")]
        public async Task<IActionResult> GetPickupPoints()
        {
            return Ok(await _adminService.GetPickupPoints());
        }

        [HttpPost("pickupPoints/save")]
        public async Task<IActionResult> SavePickupPoint(PickupPointSaveRequest req)
        {
            var res = await _adminService.SavePickupPoint(req);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses()
        {
            return Ok(await _adminService.GetStatuses());
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _adminService.GetAllOrders());
        }

        [HttpPost("orders/status")]
        public async Task<IActionResult> UpdateOrderStatus(OrderStatusUpdateRequest req)
        {
            var res = await _adminService.UpdateOrderStatus(req);
            return Ok(new { isOk = res.Item1, message = res.Item2 });
        }
    }
}

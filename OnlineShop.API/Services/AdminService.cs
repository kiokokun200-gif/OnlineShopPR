using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class AdminService : BaseService
    {
        public AdminService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            return await _context.Categories.Select(c => new CategoryDTO { CategoryID = c.CategoryId, Name = c.Name }).ToListAsync();
        }

        public async Task<(bool, string)> SaveCategory(CategorySaveRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name)) return (false, "Введите название категории");
            if (req.CategoryID.HasValue && req.CategoryID.Value > 0)
            {
                var cat = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == req.CategoryID.Value);
                if (cat == null) return (false, "Категория не найдена");
                cat.Name = req.Name;
            }
            else
            {
                var exists = await _context.Categories.FirstOrDefaultAsync(c => c.Name == req.Name);
                if (exists != null) return (false, "Такая категория уже есть");
                _context.Categories.Add(new Category { Name = req.Name });
            }
            await _context.SaveChangesAsync();
            return (true, "Категория сохранена");
        }

        public async Task<List<PickupPointDTO>> GetPickupPoints()
        {
            return await _context.PickupPoints.Select(p => new PickupPointDTO { Id = p.Id, AddressName = p.AddressName }).ToListAsync();
        }

        public async Task<(bool, string)> SavePickupPoint(PickupPointSaveRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.AddressName)) 
                return (false, "Введите адрес ПВЗ");
            if (req.Id.HasValue && req.Id.Value > 0)
            {
                var p = await _context.PickupPoints.FirstOrDefaultAsync(x => x.Id == req.Id.Value);
                if (p == null) return (false, "ПВЗ не найден");
                p.AddressName = req.AddressName;
            }
            else
            {
                _context.PickupPoints.Add(new PickupPoint { AddressName = req.AddressName });
            }
            await _context.SaveChangesAsync();
            return (true, "ПВЗ сохранен");
        }

        public async Task<List<StatusDTO>> GetStatuses()
        {
            return await _context.Statuses.Select(s => new StatusDTO { Id = s.Id, Name = s.Name }).ToListAsync();
        }

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.DeliveryType)
                .Include(o => o.PickupPoint)
                .Include(o => o.User)
                .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(o => new OrderDTO
            {
                OrderID = o.OrderId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                StatusId = o.StatusId,
                StatusName = o.Status?.Name,
                DeliveryTypeId = o.DeliveryTypeId,
                DeliveryTypeName = o.DeliveryType?.TypeName,
                AddressDelivery = o.AddressDelivery,
                PickupPointId = o.PickupPointId,
                PickupPointName = o.PickupPoint?.AddressName,
                UserLogin = o.User?.Login,
                Items = o.OrderProducts.Select(op => new OrderProductDTO
                {
                    ProductId = op.ProductId,
                    Name = op.Product?.Name,
                    Quantity = op.Quantity,
                    PurchasePrice = op.PurchasePrice
                }).ToList()
            }).ToList();
        }

        public async Task<(bool, string)> UpdateOrderStatus(OrderStatusUpdateRequest req)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == req.OrderId);
            if (order == null) return (false, "Заказ не найден");
            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == req.StatusId);
            if (status == null) return (false, "Статус не найден");
            order.StatusId = req.StatusId;
            await _context.SaveChangesAsync();
            return (true, "Статус обновлен");
        }
    }
}

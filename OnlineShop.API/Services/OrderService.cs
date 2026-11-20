using Microsoft.EntityFrameworkCore;
using OnlineShop.API.Models;
using OnlineShop.Core.DTOs.DB;

namespace OnlineShop.API.Services
{
    public class OrderService : BaseService
    {
        public OrderService(OnlineShopContext shopContext) : base(shopContext)
        {
        }

        public async Task<List<PickupPointDTO>> GetPickupPoints()
        {
            return await _context.PickupPoints.Select(p => new PickupPointDTO { Id = p.Id, AddressName = p.AddressName }).ToListAsync();
        }

        public async Task<List<OrderDTO>> GetUserOrders(int userId, string sort = "new")
        {
            var q = _context.Orders
                .Include(o => o.Status)
                .Include(o => o.DeliveryType)
                .Include(o => o.PickupPoint)
                .Include(o => o.User)
                .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                .Where(o => o.UserId == userId);

            q = sort == "old" ? q.OrderBy(o => o.OrderDate) : q.OrderByDescending(o => o.OrderDate);
            var orders = await q.ToListAsync();

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

        public async Task<(bool, string)> CreateOrder(int userId, CreateOrderRequest req)
        {
            var korzina = await _context.Korzinas.Include(k => k.KorzinaProducts).ThenInclude(kp => kp.Product)
                .FirstOrDefaultAsync(k => k.UserId == userId);
            if (korzina == null || korzina.KorzinaProducts.Count == 0)
                return (false, "Корзина пуста");

            List<KorzinaProduct> items;
            if (req.ProductId.HasValue)
            {
                items = korzina.KorzinaProducts.Where(k => k.ProductId == req.ProductId.Value).ToList();
                if (items.Count == 0) return (false, "Товар не найден в корзине");
            }
            else
            {
                items = korzina.KorzinaProducts.ToList();
            }

            foreach (var item in items)
            {
                if (!item.Product.IsActive || item.Product.QuantityInStock < item.Quantity)
                    return (false, $"Товар '{item.Product.Name}' недоступен или его недостаточно на складе");
            }

            decimal total = items.Sum(i => i.Quantity * i.Product.Price);

            string deliveryTypeName;
            int? pickupPointId = null;
            string? address = null;

            if (total > 600)
            {
                if (req.IsDelivery)
                {
                    if (string.IsNullOrWhiteSpace(req.AddressDelivery)) return (false, "Введите адрес доставки");
                    deliveryTypeName = "Доставка";
                    address = req.AddressDelivery;
                }
                else
                {
                    if (!req.PickupPointId.HasValue) return (false, "Выберите ПВЗ");
                    var pvz = await _context.PickupPoints.FirstOrDefaultAsync(p => p.Id == req.PickupPointId.Value);
                    if (pvz == null) return (false, "ПВЗ не найден");
                    deliveryTypeName = "Самовывоз";
                    pickupPointId = pvz.Id;
                }
            }
            else
            {
                if (req.IsDelivery) return (false, "Доставка доступна только при сумме заказа больше 600 ₽");
                if (!req.PickupPointId.HasValue) return (false, "Выберите ПВЗ");
                var pvz = await _context.PickupPoints.FirstOrDefaultAsync(p => p.Id == req.PickupPointId.Value);
                if (pvz == null) return (false, "ПВЗ не найден");
                deliveryTypeName = "Самовывоз";
                pickupPointId = pvz.Id;
            }

            var deliveryType = await _context.DeliveryTypes.FirstOrDefaultAsync(d => d.TypeName == deliveryTypeName);
            if (deliveryType == null)
            {
                deliveryType = new DeliveryType { TypeName = deliveryTypeName };
                _context.DeliveryTypes.Add(deliveryType);
                await _context.SaveChangesAsync();
            }

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == "Оформлен");
            if (status == null)
            {
                status = new Status { Name = "Оформлен" };
                _context.Statuses.Add(status);
                await _context.SaveChangesAsync();
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = total,
                StatusId = status.Id,
                DeliveryTypeId = deliveryType.Id,
                PickupPointId = pickupPointId,
                AddressDelivery = address
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                _context.OrderProducts.Add(new OrderProduct
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PurchasePrice = item.Product.Price
                });

                item.Product.QuantityInStock -= item.Quantity;
                _context.KorzinaProducts.Remove(item);
            }

            await _context.SaveChangesAsync();

            var left = await _context.KorzinaProducts.Where(k => k.KorzinaId == korzina.KorzinaId).Include(k => k.Product).ToListAsync();
            korzina.TotalPrice = left.Sum(k => k.Quantity * k.Product.Price);
            await _context.SaveChangesAsync();

            return (true, "Заказ успешно оформлен");
        }
    }
}

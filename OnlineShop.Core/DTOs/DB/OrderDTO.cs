using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? StatusName { get; set; }
        public int StatusId { get; set; }
        public string? DeliveryTypeName { get; set; }
        public int? DeliveryTypeId { get; set; }
        public string? AddressDelivery { get; set; }
        public int? PickupPointId { get; set; }
        public string? PickupPointName { get; set; }
        public string? UserLogin { get; set; }
        public List<OrderProductDTO> Items { get; set; } = new List<OrderProductDTO>();
    }
}

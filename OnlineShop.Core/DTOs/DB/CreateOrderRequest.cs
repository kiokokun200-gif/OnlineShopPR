using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class CreateOrderRequest
    {
        public int? ProductId { get; set; }
        public bool IsDelivery { get; set; }
        public int? PickupPointId { get; set; }
        public string? AddressDelivery { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class OrderProductDTO
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public string FullDescription => $"{Name} - {Quantity} шт. x {PurchasePrice} ₽";
    }
}

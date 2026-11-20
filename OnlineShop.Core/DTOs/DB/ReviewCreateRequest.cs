using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class ReviewCreateRequest
    {
        public int ProductId { get; set; }
        public decimal Rating { get; set; }
        public string ReviewText { get; set; } = "";
    }
}

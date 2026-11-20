using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? UserLogin { get; set; }
        public string ReviewText { get; set; } = "";
        public string Text
        {
            get => ReviewText;
            set => ReviewText = value;
        }
        public decimal Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}

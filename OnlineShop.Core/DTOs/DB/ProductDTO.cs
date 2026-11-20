using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CategoriName { get; set; }

        public decimal Price { get; set; }

        [JsonIgnore]
        private string? _pathImg = "/images/products/default.png";
        public string? PathImg
        {
            get
            {
                return _pathImg;
            }
            set
            {
                if (_pathImg != value || value != null)
                    _pathImg = value;
            }
        } 

        public decimal? Rating { get; set; } = 0;

        public int QuantityInStock { get; set; }

        public int QuantityInPack { get; set; }

        public int ReviewCount { get; set; }

        public bool IsFavorite { get; set; }

        public bool isActive { get; set; }
        public bool isInCart { get; set; }

        public int quantityInCart { get; set; } = 0;

    }
}

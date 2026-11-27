using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class PickupPointSaveRequest
    {
        public int? Id { get; set; }
        public string AddressName { get; set; } = "";
    }
}

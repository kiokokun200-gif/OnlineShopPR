using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.DB
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string RoleName { get; set; }
    }
}

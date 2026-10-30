using OnlineShop.Core.DTOs.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.Auth
{
    public class LoginResponse
    {
        public string message { get; set; }
        public bool isLogin {  get; set; }
        public string? token { get; set; }
        public UserDTO? user { get; set; }
    }
}

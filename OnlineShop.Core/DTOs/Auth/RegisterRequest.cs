using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.Auth
{
    public class RegisterRequest
    {
        public string login {  get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; } = "";

    }
}

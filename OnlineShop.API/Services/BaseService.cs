using OnlineShop.API.Models;

namespace OnlineShop.API.Services
{
    public class BaseService(OnlineShopContext shopContext)
    {
        protected readonly OnlineShopContext _context = shopContext;
    }
}

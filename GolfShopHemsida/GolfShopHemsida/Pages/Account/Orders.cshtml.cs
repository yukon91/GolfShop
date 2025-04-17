using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GolfShopHemsida.Pages.Account
{
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<Order> Orders { get; set; }

        public OrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}

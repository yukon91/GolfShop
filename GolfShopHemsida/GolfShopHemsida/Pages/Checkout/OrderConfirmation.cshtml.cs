using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfShopHemsida.Services;
using GolfShopHemsida.Models;
using Microsoft.EntityFrameworkCore;

namespace GolfShopHemsida.Pages.Checkout
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrderConfirmationModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(OrderId))
                return RedirectToPage("/Index");

            if (!int.TryParse(OrderId, out int parsedOrderId))
                return RedirectToPage("/Index");

            Order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.OrderId == parsedOrderId);

            if (Order == null)
                return RedirectToPage("/Index");

            return Page();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GolfShopHemsida.Models;

public class OrderHistoryModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly UserManager<GolfShopUser> _userManager;

    public OrderHistoryModel(AppDbContext context, UserManager<GolfShopUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<Order> Orders { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            Orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();
        }
        else
        {
            Orders = new List<Order>();
        }
    }
}
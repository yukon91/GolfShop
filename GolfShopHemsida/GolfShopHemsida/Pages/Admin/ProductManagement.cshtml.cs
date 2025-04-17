using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ProductManagementModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProductManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Item> Items { get; set; } = new();

        public void OnGet()
        {
            Items = _context.Items.ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

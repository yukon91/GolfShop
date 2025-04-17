using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GolfShopHemsida.Models;
using GolfShopHemsida.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GolfShopHemsida.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class PurchaseHistoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public PurchaseHistoryModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Purchase> Purchases { get; set; }



        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            Purchases = await _context.Purchases
                .Include(p => p.User)
                .Where(p => p.GolfShopUserId == id)
                .ToListAsync();

            return Page();
        }
    }
}
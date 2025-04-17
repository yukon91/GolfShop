using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GolfShopHemsida.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GolfShopHemsida.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUserModel : PageModel
    {
        private readonly UserManager<GolfShopUser> _userManager;
        private readonly SignInManager<GolfShopUser> _signInManager;
        private readonly AppDbContext _context;

        public AdminUserModel(
            UserManager<GolfShopUser> userManager,
            SignInManager<GolfShopUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IList<GolfShopUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var followRelations = await _context.FollowUsers
                .Where(f => f.FollowerId == id || f.FollowedId == id)
                .ToListAsync();
            _context.FollowUsers.RemoveRange(followRelations);

            var cartItems = await _context.CartItems
                .Where(ci => ci.ShoppingCart.UserId == id)
                .ToListAsync();
            _context.CartItems.RemoveRange(cartItems);

            var cart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == id);
            if (cart != null)
            {
                _context.ShoppingCarts.Remove(cart);
            }

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                Users = await _userManager.Users.ToListAsync();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
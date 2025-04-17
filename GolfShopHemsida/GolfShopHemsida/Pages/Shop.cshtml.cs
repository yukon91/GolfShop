using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using GolfShopHemsida.Models;
using GolfShopHemsida.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace GolfShopHemsida.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ShoppingCartService _cartService;
        private readonly AppDbContext _context;
        private readonly UserManager<GolfShopUser> _userManager;

        public ShopModel(ShoppingCartService cartService, AppDbContext context, UserManager<GolfShopUser> userManager)
        {
            _cartService = cartService;
            _context = context;
            _userManager = userManager;

            Items = new List<Item>();
            CartItems = new List<CartItem>();
        }

        public List<Item> Items { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int CartItemCount => CartItems.Sum(item => item.Quantity); 
        public decimal CartTotal => CartItems.Sum(item => item.Quantity * item.Item.Price); 

        public async Task OnGetAsync()
        {
            Items = await _context.Items
            .Include(i => i.Comments)
            .ThenInclude(c => c.User)
            .ToListAsync();

            var cart = await _cartService.GetUserCart();
            CartItems = cart.CartItems;
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string itemId)
        {
            try
            {
                await _cartService.AddToCart(itemId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(string cartItemId)
        {
            try
            {
                await _cartService.RemoveFromCart(cartItemId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddCommentAsync(string itemId, string content)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (string.IsNullOrEmpty(itemId))
            {
                return BadRequest("Item ID is required.");
            }

            var item = await _context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
            if (item == null)
            {
                return NotFound(); 
            }

            var comment = new Comment
            {
                CommentId = Guid.NewGuid().ToString(),
                Content = content,
                GolfShopUserId = user.Id,
                ItemId = itemId, 
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
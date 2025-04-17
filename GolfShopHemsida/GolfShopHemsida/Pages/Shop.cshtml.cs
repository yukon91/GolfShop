using GolfShopHemsida.Models;
using GolfShopHemsida.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ShoppingCartService _cartService;

        public ShopModel(ShoppingCartService cartService)
        {
            _cartService = cartService;
            Items = new List<Item>();
            CartItems = new List<CartItem>();
        }

        public List<Item> Items { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int CartItemCount => CartItems.Sum(item => item.Quantity); // Calculates total items in the cart
        public decimal CartTotal => CartItems.Sum(item => item.Quantity * item.Item.Price); // Calculates total price of the cart

        public async Task OnGetAsync()
        {
            // Fetch items from the database
            Items = await _cartService.GetAvailableItemsAsync();

            // Fetch the user's cart
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
    }
}
using GolfShopHemsida.Models;
using GolfShopHemsida.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfShopHemsida.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ShoppingCartService _cartService;

        public ShoppingCart Cart { get; set; }

        public IndexModel(ShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public async Task OnGetAsync()
        {
            Cart = await _cartService.GetUserCart();
        }

        public async Task<IActionResult> OnPostRemoveFromCart(string cartItemId)
        {
            await _cartService.RemoveFromCart(cartItemId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckout()
        {
            var order = await _cartService.Checkout();
            return RedirectToPage("/Cart/OrderConfirmation", new { orderId = order.OrderId });
        }
        public async Task<IActionResult> OnPostAddToCart(string itemId)
        {
            try
            {
                await _cartService.AddToCart(itemId);
                TempData["SuccessMessage"] = "Product added to cart successfully";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding to cart";
                // Log the exception here
            }

            return RedirectToPage();
        }
    }
}

using GolfShopHemsida.Models;
using GolfShopHemsida.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GolfShopHemsida.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly ShoppingCartService _cartService;

        public IndexModel(ShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public ShoppingCart Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { returnUrl = "/Checkout" });
            }

            Cart = await _cartService.GetUserCart();

            if (!Cart.CartItems.Any())
            {
                return RedirectToPage("/Cart/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(string cartItemId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            await _cartService.RemoveFromCart(cartItemId);
            return RedirectToPage(); // Refreshes the current checkout page
        }

        public async Task<IActionResult> OnPostPlaceOrderAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetUserCart();

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToPage("/Cart/Index");
            }

            // Create a new order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(item => item.Item.Price * item.Quantity),
                Status = "Pending",
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Item.Price // Important for order history display
                }).ToList()
            };

            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Orders.Add(order);
                dbContext.CartItems.RemoveRange(cart.CartItems); // Clear cart
                await dbContext.SaveChangesAsync();
            }

            return RedirectToPage("/Checkout/OrderConfirmation", new { orderId = order.OrderId });
        }


    }
}
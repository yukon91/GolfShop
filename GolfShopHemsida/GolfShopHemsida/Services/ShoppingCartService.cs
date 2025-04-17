using GolfShopHemsida.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GolfShopHemsida.Services
{
    public class ShoppingCartService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ShoppingCart
                {
                    UserId = null,
                    CartItems = new List<CartItem>()
                };
            }

            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddToCart(string itemId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("User not logged in.");

            var cart = await GetUserCart();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ItemId == itemId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                var item = await _context.Items.FindAsync(itemId);
                if (item == null || item.Stock <= 0)
                    throw new InvalidOperationException("Item not available.");

                cart.CartItems.Add(new CartItem
                {
                    ItemId = itemId,
                    Quantity = 1,
                    Item = item
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCart(string cartItemId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (item == null)
            {
                throw new InvalidOperationException("Item not found in cart");
            }

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> Checkout()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User must be logged in to checkout");
            }

            var cart = await GetUserCart();

            if (!cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cannot checkout an empty cart");
            }

            // Verify all items still exist and have stock  
            foreach (var cartItem in cart.CartItems)
            {
                var item = await _context.Items.FindAsync(cartItem.ItemId);
                if (item == null || item.Stock < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Product {cartItem.ItemId} is no longer available or out of stock");
                }

                // Deduct stock  
                item.Stock -= cartItem.Quantity;
            }

            var order = new Order
            {
                OrderId = 0, // Fix: Set to 0 for auto-incremented primary key  
                UserId = cart.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(i => i.Item.Price * i.Quantity),
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cart.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                    ItemId = cartItem.ItemId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Item.Price
                });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Orders.Add(order);
                _context.ShoppingCarts.Remove(cart);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<Item>> GetAvailableItemsAsync()
        {
            return await _context.Items
                .Where(item => item.Stock > 0) // Only items with stock greater than 0
                .ToListAsync();
        }
    }
}
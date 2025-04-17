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

        public async Task<ShoppingCart> GetUserCart(string? userId)
        {
            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new InvalidOperationException("User must be logged in to access cart");
            }

            // Verify user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == currentUserId);
            if (!userExists)
            {
                throw new InvalidOperationException("User account not found");
            }

            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync(c => c.UserId == currentUserId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = currentUserId,
                    CartItems = new List<CartItem>()
                };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddToCart(string itemId, int quantity = 1)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User must be logged in");
            }

            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                throw new InvalidOperationException("Product not found");
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
            }

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ItemId == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ItemId = itemId,
                    Quantity = quantity,
                    ShoppingCartId = cart.ShoppingCartId
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

            var cart = await GetUserCart(userId);

            if (!cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cannot checkout empty cart");
            }

            // Verify all items still exist and have stock
            foreach (var cartItem in cart.CartItems)
            {
                var item = await _context.Items.FindAsync(cartItem.ItemId);
                if (item == null)
                {
                    throw new InvalidOperationException($"Product {cartItem.ItemId} no longer available");
                }
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
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
    }
}
using GolfShopHemsida.Models;
using GolfShopHemsida.Repositories;
using GolfShopHemsida.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GolfShopHemsida.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ItemRepository _itemRepository;
        private readonly ShoppingCartService _cartService;

        public ShopModel(ItemRepository itemRepository, ShoppingCartService cartService)
        {
            _itemRepository = itemRepository;
            _cartService = cartService;
            Items = new List<Item>();
        }



        public List<Item> Items { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            var items = await _itemRepository.GetAllItemsAsync();
            Items = items.Select(item => new Item
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                Stock = item.Stock
            }).ToList();
        }



        public async Task<JsonResult> OnPostAddToCartAsync(string itemId)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Du måste logga in först"
                    });
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Kunde inte identifiera användaren"
                    });
                }

                await _cartService.AddToCart(itemId);

                // Return updated cart data  
                var cart = await _cartService.GetUserCart(userId);
                return new JsonResult(new
                {
                    success = true,
                    count = cart.CartItems.Sum(i => i.Quantity),
                    total = cart.CartItems.Sum(i => i.Quantity * i.Item.Price)
                });
            }
            catch (InvalidOperationException ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = $"Ett fel uppstod: {ex.Message}"
                });
            }
        }


        // Cart-related properties (optional - can be removed if using ShoppingCartService directly)
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public int CartItemCount => CartItems.Sum(item => item.Quantity);
        public decimal CartTotal => CartItems.Sum(item => item.Quantity * item.Item.Price);

        public class CartItem
        {
            public string CartItemId { get; set; }
            public Item Item { get; set; }
            public int Quantity { get; set; }
        }

        public class Item
        {
            public string ItemId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
            public int Stock { get; set; }
        }
    }
}
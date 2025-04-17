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

        public async Task<IActionResult> OnPostAddToCartAsync(string itemId)
        {
            try
            {
                await _cartService.AddToCart(itemId);
                SuccessMessage = "Item successfully added to your cart!";
            }
            catch (InvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while adding item to cart: " + ex.Message;
            }

            return RedirectToPage("/Shop/Index");
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
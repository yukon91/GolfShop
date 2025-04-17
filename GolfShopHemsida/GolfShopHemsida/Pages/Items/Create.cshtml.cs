using GolfShopHemsida.Models;
using GolfShopHemsida.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfShopHemsida.Pages.Items
{
    public class CreateModel : PageModel
    {
        private readonly ItemRepository _itemRepository;

        public CreateModel(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            Item = new Item(); // Initialize Item to avoid nullability issues
        }

        [BindProperty]
        public Item Item { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _itemRepository.AddItemAsync(Item);

            return RedirectToPage("./Index");
        }
    }
}
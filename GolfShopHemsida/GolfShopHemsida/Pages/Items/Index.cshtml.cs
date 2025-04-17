using GolfShopHemsida.Models;
using GolfShopHemsida.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfShopHemsida.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly ItemRepository _itemRepository;

        public IndexModel(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public List<Item> Items { get; set; }

        public async Task OnGetAsync()
        {
            Items = await _itemRepository.GetAllItemsAsync();
        }
    }
}

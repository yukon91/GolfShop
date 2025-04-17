using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfShopHemsida.Pages.Cart
{
    public class OrderConfirmationModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string OrderId { get; set; }

        public void OnGet()
        {
        }
    }
}

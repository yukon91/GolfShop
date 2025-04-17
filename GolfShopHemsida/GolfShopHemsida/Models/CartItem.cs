using System.ComponentModel.DataAnnotations;

namespace GolfShopHemsida.Models
{
    public class CartItem
    {
        [Key]
        public string CartItemId { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public string ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; } = 1;
    }
}

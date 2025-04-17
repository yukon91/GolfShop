namespace GolfShopHemsida.Models
{
    public class ShoppingBag
    {
        public string ShoppingBagId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public GolfShopUser User { get; set; }

        public ICollection<ShoppingBagItem> ShoppingBagItems { get; set; }
    }
}

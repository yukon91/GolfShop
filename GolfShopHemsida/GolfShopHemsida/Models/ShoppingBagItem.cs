namespace GolfShopHemsida.Models
{
    public class ShoppingBagItem
    {
        public string ShoppingBagItemId { get; set; }
        public int Quantity { get; set; }

        public string ShoppingBagId { get; set; }
        public ShoppingBag ShoppingBag { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}

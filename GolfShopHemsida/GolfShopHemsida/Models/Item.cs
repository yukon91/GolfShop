namespace GolfShopHemsida.Models
{
    public class Item
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

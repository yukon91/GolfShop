namespace GolfShopHemsida.Models
{
    public class OrderItem
    {
        public string OrderItemId { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

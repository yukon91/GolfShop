namespace GolfShopHemsida.Models
{
    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public GolfShopUser User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Completed";
        public string GolfShopUserId { get; internal set; }
    }
}

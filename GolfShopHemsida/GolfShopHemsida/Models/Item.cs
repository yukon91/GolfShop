using System.ComponentModel.DataAnnotations;

namespace GolfShopHemsida.Models
{
    public class Item
    {
        [Key]
        public string ItemId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

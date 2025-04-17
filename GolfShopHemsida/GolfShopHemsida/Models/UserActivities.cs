using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GolfShopHemsida.Models
{
    public class UserActivities
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public GolfShopUser Receiver { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public string? PostId { get; set; }
        public Post? Post { get; set; }

        public string? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }

}

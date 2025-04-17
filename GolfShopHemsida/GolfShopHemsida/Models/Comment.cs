using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GolfShopHemsida.Models
{
    public class Comment
    {
        [Key]
        public string CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string GolfShopUserId { get; set; }

        [ForeignKey(nameof(GolfShopUserId))]
        public GolfShopUser User { get; set; }
        public string? PostId { get; set; }
        public Post? Post { get; set; } 
        public string? ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item? Item { get; set; } 
    }
}
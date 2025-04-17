using System;
using System.Collections.Generic;
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
        public GolfShopUser User { get; set; }

        public string PostId { get; set; } 
        public Post Post { get; set; }
    }
}

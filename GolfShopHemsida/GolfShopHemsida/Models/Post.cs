using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GolfShopHemsida.Models
{
    public class Post
    {
        [Key]
        public string PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string GolfShopUserId { get; set; }
        public GolfShopUser User { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }

}

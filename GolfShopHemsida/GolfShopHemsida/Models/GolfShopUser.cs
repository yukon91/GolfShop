using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GolfShopHemsida.Models
{
    public class GolfShopUser : IdentityUser
    {
        public string? ProfilePicture { get; set; }
        public string? Namn { get; set; }
        public string? Adress { get; set; }
        public string? Betalsätt { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}

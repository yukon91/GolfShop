namespace GolfShopHemsida.Models
{
    public class FollowUser
    {
        public string FollowerId { get; set; } = string.Empty; 
        public GolfShopUser? Follower { get; set; }

        public string FollowedId { get; set; } = string.Empty; 
        public GolfShopUser? Followed { get; set; }
    }
}

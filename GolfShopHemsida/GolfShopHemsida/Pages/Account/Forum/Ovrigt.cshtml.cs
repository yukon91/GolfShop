using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages.Account.Forum
{
    public class OvrigtModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<GolfShopUser> _userManager;

        public OvrigtModel(AppDbContext context, UserManager<GolfShopUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<UserActivities> Notifications { get; set; }
        public List<Post> Threads { get; set; }
        public string CurrentUserId { get; set; }

        //  Get posts & comments
        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            CurrentUserId = currentUser?.Id;

            Threads = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .Where(p => p.Category == "Ovrigt")
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            Notifications = await _context.UserActivities
                .Where(n => n.ReceiverId == currentUser.Id && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Page();
        }

        // Creat new post and let followers know
        public async Task<IActionResult> OnPostCreatePostAsync(string title, string content)
        {

            var currentUser = await _userManager.GetUserAsync(User);

            var post = new Post
            {
                Title = title,
                Content = content,
                Category = "Ovrigt",
                PostId = Guid.NewGuid().ToString(),
                GolfShopUserId = currentUser.Id
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var followers = await _context.FollowUsers
                .Where(f => f.FollowedId == currentUser.Id)
                .Select(f => f.FollowerId)
                .ToListAsync();

            foreach (var followerId in followers)
            {
                var follower = await _userManager.FindByIdAsync(followerId);
                var activity = new UserActivities
                {
                    ReceiverId = followerId,
                    Message = $"{currentUser.Namn} created a new post: {title}",
                    PostId = post.PostId,
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };

                _context.UserActivities.Add(activity);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteThreadAsync(string threadId)
        {
            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == threadId);

            if (post == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (post.GolfShopUserId != currentUser.Id)
            {
                return Forbid();
            }

            var userActivities = _context.UserActivities
                .Where(ua => ua.PostId == post.PostId);

            _context.UserActivities.RemoveRange(userActivities);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(string PostId, string Comment)
        {
            if (string.IsNullOrWhiteSpace(Comment))
            {
                return RedirectToPage();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.PostId == PostId);
            if (post == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                CommentId = Guid.NewGuid().ToString(),
                Content = Comment,
                CreatedAt = DateTime.Now,
                GolfShopUserId = user.Id,
                PostId = post.PostId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var followers = await _context.FollowUsers
                .Where(f => f.FollowedId == user.Id)
                .Select(f => f.FollowerId)
                .ToListAsync();

            foreach (var followerId in followers)
            {
                var activity = new UserActivities
                {
                    ReceiverId = followerId,
                    Message = $"{user.Namn} commented on a post.",
                    CommentId = comment.CommentId,
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };

                _context.UserActivities.Add(activity);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteCommentAsync(string commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (comment.GolfShopUserId != currentUser.Id)
            {
                return Forbid();
            }

            var relatedActivities = _context.UserActivities.Where(ua => ua.CommentId == commentId);
            _context.UserActivities.RemoveRange(relatedActivities);
            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}

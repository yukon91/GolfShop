using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages.Account.Forum
{
    public class GolfRelateratModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<GolfShopUser> _userManager;

        public GolfRelateratModel(AppDbContext context, UserManager<GolfShopUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
                .Where(p => p.Category == "GolfRelaterat")
                .ToListAsync();

            return Page();
        }

        // Creat new post
        public async Task<IActionResult> OnPostCreatePostAsync(string title, string content)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var post = new Post
            {
                Title = title,
                Content = content,
                Category = "GolfRelaterat",
                PostId = Guid.NewGuid().ToString(),
                GolfShopUserId = currentUser.Id
            };

            _context.Posts.Add(post);
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

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}

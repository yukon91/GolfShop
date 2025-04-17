using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfShopHemsida.Pages.Account
{
    [Authorize]
    public class MemberModel : PageModel
    {
        private readonly UserManager<GolfShopUser> _userManager;
        private readonly SignInManager<GolfShopUser> _signInManager;
        private readonly AppDbContext _context;

        public MemberModel(UserManager<GolfShopUser> userManager, SignInManager<GolfShopUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public string? ProfilePicturePath { get; set; }
        [BindProperty]
        public IFormFile? ProfilePicture { get; set; }
        [BindProperty]
        public string? Namn { get; set; }
        [BindProperty]
        public string? Adress { get; set; }
        public string? Email { get; set; }
        [BindProperty]
        public string? Betalsätt { get; set; }

        public List<UserActivities> Notifications { get; set; } = new List<UserActivities>();
        public List<GolfShopUser> OtherUsers { get; set; } = new List<GolfShopUser>();
        public List<FollowUser> Followers { get; set; } = new List<FollowUser>();
        public List<FollowUser> Following { get; set; } = new List<FollowUser>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var allUsers = _userManager.Users.Where(u => u.Id != user.Id).ToList();
            OtherUsers = new List<GolfShopUser>();

            foreach (var u in allUsers)
            {
                if (!await _userManager.IsInRoleAsync(u, "Admin"))
                {
                    OtherUsers.Add(u);
                }
            }

            Followers = _context.FollowUsers.Where(f => f.FollowedId == user.Id).ToList();
            Following = _context.FollowUsers.Where(f => f.FollowerId == user.Id).ToList();

            var allNotifications = await _context.UserActivities
                .Include(n => n.Post)
                .ThenInclude(p => p.User) 
                .Include(n => n.Comment)
                .ThenInclude(c => c.User)  
                .Where(n => n.ReceiverId == user.Id && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            var followingIds = Following.Select(f => f.FollowedId).ToList();
            Notifications = allNotifications.Where(n =>
                (n.Post != null && followingIds.Contains(n.Post.User.Id)) ||
                (n.Comment != null && followingIds.Contains(n.Comment.User.Id)))
                .ToList();

            Namn = user.Namn;
            Adress = user.Adress;
            Email = user.Email;
            Betalsätt = user.Betalsätt;
            ProfilePicturePath = user.ProfilePicture;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            user.Namn = Namn;
            user.Adress = Adress;
            user.Email = Email;
            user.Betalsätt = Betalsätt;

            if (ProfilePicture != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "Profile_Pictures");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ProfilePicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = $"/images/Profile_Pictures/{fileName}";
            }

            await _userManager.UpdateAsync(user);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFollowAsync(string followId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || followId == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            if (user.Id == followId)
            {
                return RedirectToPage();
            }

            var existingFollow = _context.FollowUsers
                .FirstOrDefault(f => f.FollowerId == user.Id && f.FollowedId == followId);

            if (existingFollow == null)
            {
                var followUser = new FollowUser
                {
                    FollowerId = user.Id,
                    FollowedId = followId
                };

                _context.FollowUsers.Add(followUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnfollowAsync(string followId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || followId == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            var follow = _context.FollowUsers
                .FirstOrDefault(f => f.FollowerId == user.Id && f.FollowedId == followId);

            if (follow != null)
            {
                _context.FollowUsers.Remove(follow);

                var notifications = await _context.UserActivities
                    .Where(n => n.ReceiverId == user.Id && !n.IsRead) 
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    if (notification.Message.Contains(followId)) 
                    {
                        notification.IsRead = true; 
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(); 
        }


        public async Task<IActionResult> OnPostLogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}

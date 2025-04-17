using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages.Account
{
    [Authorize]
    public class MemberModel : PageModel
    {
        private readonly UserManager<GolfShopUser> _userManager;
        private readonly SignInManager<GolfShopUser> _signInManager;

        public MemberModel(UserManager<GolfShopUser> userManager, SignInManager<GolfShopUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
        }

        [BindProperty]
        public string? ProfilePicturePath { get; set; }
        [BindProperty]
        public IFormFile? ProfilePicture { get; set; }
        [BindProperty]
        public string? Namn { get; set; }
        [BindProperty]
        public string? Adress { get; set; }
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Betalsätt { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }

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

        public async Task<IActionResult> OnPostLogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GolfShopHemsida.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GolfShopHemsida.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUserModel : PageModel
    {
        private readonly UserManager<GolfShopUser> _userManager;
        private readonly SignInManager<GolfShopUser> _signInManager;

        public AdminUserModel(UserManager<GolfShopUser> userManager, SignInManager<GolfShopUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IList<GolfShopUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
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
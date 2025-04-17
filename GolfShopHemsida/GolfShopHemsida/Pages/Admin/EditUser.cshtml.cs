using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<GolfShopUser> _userManager;

        public EditUserModel(UserManager<GolfShopUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public GolfShopUser UserToEdit { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            UserToEdit = await _userManager.FindByIdAsync(id);

            if (UserToEdit == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByIdAsync(UserToEdit.Id);
            if (user == null)
                return NotFound();

            user.Email = UserToEdit.Email;
            user.UserName = UserToEdit.UserName;
            user.PhoneNumber = UserToEdit.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            return RedirectToPage("/Admin/AdminUser");
        }
    }
}

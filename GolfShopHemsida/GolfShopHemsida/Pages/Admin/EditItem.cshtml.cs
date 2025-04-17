using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;

namespace GolfShopHemsida.Pages.Admin
{
    public class EditItemModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditItemModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Item ItemToEdit { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return NotFound();
            }

            ItemToEdit = await _context.Items.FindAsync(itemId);

            if (ItemToEdit == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingItem = await _context.Items.FindAsync(ItemToEdit.ItemId);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = ItemToEdit.Name;
            existingItem.Description = ItemToEdit.Description;
            existingItem.Price = ItemToEdit.Price;
            existingItem.Stock = ItemToEdit.Stock;

            if (Image != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images", "Shop");
                var uniqueFileName = Path.GetFileNameWithoutExtension(Image.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                existingItem.ImageUrl = $"/Images/Shop/{uniqueFileName}";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest();
            }

            return RedirectToPage("/Admin/ProductManagement");
        }
    }
}
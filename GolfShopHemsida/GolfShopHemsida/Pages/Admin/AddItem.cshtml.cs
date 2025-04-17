using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GolfShopHemsida.Pages.Admin
{
    public class AddItemModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AddItemModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public ItemInputModel AddItem { get; set; } = new ItemInputModel();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string? imagePath = null;

            if (Image != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images", "Shop");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                imagePath = $"/Images/Shop/{uniqueFileName}";
            }
            var newItem = new Item
            {
                ItemId = Guid.NewGuid().ToString(),
                Name = AddItem.Name,
                Description = AddItem.Description,
                Price = AddItem.Price,
                Stock = AddItem.Stock,
                ImageUrl = imagePath
            };

            _context.Items.Add(newItem);
            _context.SaveChanges();

            return RedirectToPage("/Admin/ProductManagement");
        }
    }
}
using GolfShopHemsida.Models;
using Microsoft.AspNetCore.Identity;

namespace GolfShopHemsida.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<GolfShopUser> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = new GolfShopUser
            {
                UserName = "admin@hotmail.com",
                Email = "admin@hotmail.com",
            };

            string adminPassword = "AdminPass1!";
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);

            if (createAdminUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                foreach (var error in createAdminUser.Errors)
                {
                    Console.WriteLine($"Failed: {error.Description}");
                }
            }                 
        }
    }
}
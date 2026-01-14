using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using spark.Models;

namespace spark.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Ensure DB exists
            await context.Database.EnsureCreatedAsync();

            // =========================
            // 1️⃣ Seed Admin Role
            // =========================
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // =========================
            // 2️⃣ Assign Admin to First User
            // =========================
            var adminsExist = (await userManager.GetUsersInRoleAsync("Admin")).Any();
            if (!adminsExist)
            {
                var firstUser = await userManager.Users.OrderBy(u => u.Id).FirstOrDefaultAsync();
                if (firstUser != null)
                {
                    await userManager.AddToRoleAsync(firstUser, "Admin");
                }
            }

            // =========================
            // 3️⃣ Seed Products (Only Once)
            // =========================
            if (context.Computers.Any())
            {
                Console.WriteLine("Database already seeded.");
                return;
            }

            var hp = new Computer
            {
                Name = "HP Pavillion",
                Price = 500,
                Description = "Standard configuration",
                ImageUrl = "./public/hp_pav.webp"
            };

            var imac = new Computer
            {
                Name = "Imac",
                Price = 1000,
                Description = "Apple desktop computer",
                ImageUrl = "./public/imac.jpeg"
            };

            var macAir = new Computer
            {
                Name = "Macbook Air",
                Price = 5000,
                Description = "Lightweight Apple laptop",
                ImageUrl = "./public/mc_air.jpeg"
            };

            context.Computers.AddRange(hp, imac, macAir);
            await context.SaveChangesAsync();

            var components = new[]
            {
                new Component { Name = "8GB RAM", Price = 50, Type = "RAM", Computer = hp },
                new Component { Name = "16GB RAM", Price = 100, Type = "RAM", Computer = imac },
                new Component { Name = "500GB SSD", Price = 75, Type = "Storage", Computer = hp },
                new Component { Name = "1TB SSD", Price = 150, Type = "Storage", Computer = imac }
            };

            context.Components.AddRange(components);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeded successfully.");
        }
    }
}

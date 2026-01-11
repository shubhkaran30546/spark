using Microsoft.EntityFrameworkCore;
using spark.Data;
using spark.Models;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        // Ensure DB exists
        context.Database.EnsureCreated();

        // Clear existing data
        context.Components.RemoveRange(context.Components);
        context.Computers.RemoveRange(context.Computers);
        context.SaveChanges();

        // Seed computers
        // Seed computers
var hp = new Computer {
    Name = "HP Pavillion",
    Price = 500,
    Description = "Standard configuration",
    ImageUrl = "./public/hp_pav.webp"
};
var imac = new Computer {
    Name = "Imac",
    Price = 1000,
    Description = "Apple desktop computer",
    ImageUrl = "./public/imac.jpeg"
};
var macAir = new Computer {
    Name = "Macbook Air",
    Price = 5000,
    Description = "Lightweight Apple laptop",
    ImageUrl = "./public/mc_air.jpeg"
};

context.Computers.AddRange(hp, imac, macAir);
context.SaveChanges(); // IDs are now generated

// Seed components using navigation property
var components = new[]
{
    new Component { Name = "8GB RAM", Price = 50, Type = "RAM", Computer = hp },
    new Component { Name = "16GB RAM", Price = 100, Type = "RAM", Computer = imac },
    new Component { Name = "500GB SSD", Price = 75, Type = "Storage", Computer = hp },
    new Component { Name = "1TB SSD", Price = 150, Type = "Storage", Computer = imac }
};

context.Components.AddRange(components);
context.SaveChanges();


        Console.WriteLine("Database reset and seeded with new data!");
    }
}

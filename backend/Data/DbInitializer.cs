using Microsoft.EntityFrameworkCore;
using spark.Data;
using spark.Models;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        context.Database.EnsureCreated();

        if (!context.Computers.Any())
        {
            var computers = new[]
            {
                new Computer {
    Name = "HP Pavillion",
    Price = 500,
    Description = "Standard configuration",
    ImageUrl = "/uploads/products/hp_pav.webp"
},
new Computer {
    Name = "Imac",
    Price = 1000,
    Description = "Apple desktop computer",
    ImageUrl = "/uploads/products/imac.jpeg"
},
new Computer {
    Name = "Macbook Air",
    Price = 5000,
    Description = "Lightweight Apple laptop",
    ImageUrl = "/uploads/products/mc_air.jpeg"
}

            };

            context.Computers.AddRange(computers);
            context.SaveChanges();
        }

        if (!context.Components.Any())
        {
            var components = new[]
            {
                new Component { Name = "8GB RAM", Price = 50, Type = "RAM", ComputerId = 1 },
                new Component { Name = "16GB RAM", Price = 100, Type = "RAM", ComputerId = 2 },
                new Component { Name = "500GB SSD", Price = 75, Type = "Storage", ComputerId = 1 },
                new Component { Name = "1TB SSD", Price = 150, Type = "Storage", ComputerId = 2 }
            };

            context.Components.AddRange(components);
            context.SaveChanges();
        }
    }
}

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
                new Computer { Name = "Basic PC", Price = 500, Description = "Standard configuration" },
                new Computer { Name = "Gaming PC", Price = 1000, Description = "High performance" },
                new Computer { Name = "Ultra PC", Price = 5000, Description = "High performance" }
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

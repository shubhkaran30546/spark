using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using spark.Models;

namespace spark.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderComponent> OrderComponents => Set<OrderComponent>();
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Computer → Components
            modelBuilder.Entity<Computer>()
                .HasMany(c => c.Components)
                .WithOne(c => c.Computer)
                .HasForeignKey(c => c.ComputerId);

            // Order → ApplicationUser (NOT Customer)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision
            modelBuilder.Entity<Component>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Computer>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            // Order ↔ Components (many-to-many)
            modelBuilder.Entity<OrderComponent>()
                .HasKey(oc => new { oc.OrderId, oc.ComponentId });

            modelBuilder.Entity<OrderComponent>()
            .HasOne(oc => oc.Order)
            .WithMany(o => o.OrderComponents)
            .HasForeignKey(oc => oc.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderComponent>()
                .HasOne(oc => oc.Component)
                .WithMany(c => c.OrderComponents)
                .HasForeignKey(oc => oc.ComponentId);
        }
    }
}

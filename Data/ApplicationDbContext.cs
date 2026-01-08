using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using spark.Models;

namespace spark.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<OrderComponent> OrderComponents { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
           .HasKey(c => c.Id); // Ensure the primary key is set

            modelBuilder.Entity<Computer>()
                .HasMany(c => c.Components)
                .WithOne(c => c.Computer)
                .HasForeignKey(c => c.ComputerId);

            modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Component>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Computer>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
         .HasMany(o => o.OrderComponents)
         .WithOne(oc => oc.Order)
         .HasForeignKey(oc => oc.OrderId)
         .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Component>()
            //    .HasMany(c => c.OrderComponents)
            //    .WithOne(oc => oc.Component)
            //    .HasForeignKey(oc => oc.ComponentId)
            //    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
        .HasMany(o => o.OrderComponents)
        .WithOne(oc => oc.Order)
        .HasForeignKey(oc => oc.OrderId)
        .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete

            modelBuilder.Entity<Component>()
        .HasMany(c => c.OrderComponents)
        .WithOne(oc => oc.Component)
        .HasForeignKey(oc => oc.ComponentId);

            // Optional: Configure composite key for the join entity
            modelBuilder.Entity<OrderComponent>()
                .HasKey(oc => new { oc.OrderId, oc.ComponentId });

        }


    }
}

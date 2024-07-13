using CustomerOrderSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderSystemContext.Data
{
    public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // public DbSet<User> User { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItem>()
                    .HasIndex(o => o.ProductId)
                    .IsUnique(false);

            modelBuilder.Entity<OrderItem>()
                .HasIndex(o => new { o.OrderId, o.ProductId })
                .IsUnique();
            modelBuilder.Entity<OrderItem>()
                    .Property(o => o.Price)
                    .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");
        }

    }
}








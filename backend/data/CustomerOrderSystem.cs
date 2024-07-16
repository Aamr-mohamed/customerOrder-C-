using CustomerOrderSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CustomerOrderSystem.Enums;

namespace CustomerOrderSystemContext.Data
{
    public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var user1 = new ApplicationUser
            {
                Id = "1",
                UserName = "john.doe",
                NormalizedUserName = "JOHN.DOE",
                Email = "john.doe@example.com",
                NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = Role.Customer
            };
            user1.PasswordHash = passwordHasher.HashPassword(user1, "password123");

            var user2 = new ApplicationUser
            {
                Id = "2",
                UserName = "jane.smith",
                NormalizedUserName = "JANE.SMITH",
                Email = "jane.smith@example.com",
                NormalizedEmail = "JANE.SMITH@EXAMPLE.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = Role.Sales
            };
            user2.PasswordHash = passwordHasher.HashPassword(user2, "password123");

            modelBuilder.Entity<ApplicationUser>().HasData(user1, user2);
            modelBuilder.Entity<Order>()
                .HasData(
                    new Order
                    {
                        Id = 1,
                        CustomerName = "John Doe",
                        OrderDate = DateTime.Now,
                        UserId = "1",
                    },
                    new Order
                    {
                        Id = 2,
                        CustomerName = "Jane Smith",
                        OrderDate = DateTime.Parse("2021-02-15"),
                        UserId = "2",
                    }
                );

            modelBuilder.Entity<OrderItem>()
                .HasData(
                    new OrderItem { Id = 4, OrderId = 2, ProductName = "Hat", ProductDescription = "A hat", ProductId = 4, Quantity = 2, Price = 20.0m },
                    new OrderItem { Id = 5, OrderId = 2, ProductName = "T-Shirt", ProductDescription = "its a t-shirt size 10 cheap and amazing", ProductId = 5, Quantity = 2, Price = 20.0m },
                    new OrderItem { Id = 6, OrderId = 2, ProductName = "Ball", ProductDescription = "A World class magnificent ball", ProductId = 6, Quantity = 2, Price = 20.0m },
                    new OrderItem { Id = 7, OrderId = 2, ProductName = "Shoes", ProductDescription = "A pair of shoes", ProductId = 7, Quantity = 2, Price = 20.0m }

                );

            modelBuilder.Entity<Product>()
                .HasData(
                    new Product { Id = 1, ProductName = "T-Shirt", Price = 15.75m, Description = "its a t-shirt size 10 cheap and amazing" },
                    new Product { Id = 2, ProductName = "Ball", Price = 15.75m, Description = "A World class magnificent ball" },
                    new Product { Id = 3, ProductName = "Shoes", Price = 15.75m, Description = "A pair of shoes" },
                    new Product { Id = 4, ProductName = "Hat", Price = 15.75m, Description = "A hat" },
                    new Product { Id = 5, ProductName = "T-Shirt", Price = 15.75m, Description = "its a t-shirt size 10 cheap and amazing" },
                    new Product { Id = 6, ProductName = "Ball", Price = 15.75m, Description = "A World class magnificent ball" },
                    new Product { Id = 7, ProductName = "Shoes", Price = 15.75m, Description = "A pair of shoes" },
                    new Product { Id = 8, ProductName = "Hat", Price = 15.75m, Description = "A hat" },
                    new Product { Id = 9, ProductName = "T-Shirt", Price = 15.75m, Description = "its a t-shirt size 10 cheap and amazing" }
                );

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








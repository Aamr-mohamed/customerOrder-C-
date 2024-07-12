using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CustomerOrderSystem.Models;

namespace CustomerOrderSystem.Data
{
	public class CustomerOrderSystemContext : IdentityDbContext
	{
		public CustomerOrderSystemContext(DbContextOptions<CustomerOrderSystemContext> options)
			: base(options)
		{
		}

		public DbSet<User> User { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Order>()
				.HasOne(o => o.User)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.UserId);

			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Order)
				.WithMany(o => o.OrderItems)
				.HasForeignKey(oi => oi.OrderId);
		}
	}
}


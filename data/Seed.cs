// using Microsoft.AspNetCore.Identity;
// using CustomerOrderSystem.Enums;
// using CustomerOrderSystem.Models;
//
// namespace CustomerOrderSystemContext.Data
// {
//     public static class DbInitializer
//     {
//         public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
//         {
//             context.Database.EnsureCreated();
//
//             if (context.Users.Any())
//             {
//                 return;
//             }
//
//             var users = new List<ApplicationUser>
//             {
//                 new ApplicationUser
//                 {
//                     UserName = "john doe",
//                     Email = "john.doe@example.com",
//                     Role = Role.Customer
//                 },
//                 new ApplicationUser
//                 {
//                     UserName = "jane smith",
//                     Email = "jane.smith@example.com",
//                     Role = Role.Sales
//                 },
//                 new ApplicationUser
//                 {
//                     UserName = "jane_sales",
//                     Email = "jane.sales@example.com",
//                     Role = Role.Sales
//                 }
//             };
//
//             foreach (var user in users)
//             {
//                 await userManager.CreateAsync(user, "password123");
//             }
//
//             var products = new List<Product>
//             {
//                 new Product { ProductName = "Product1", Price = 10.5m, Description = "Description1" },
//                 new Product { ProductName = "Product2", Price = 20.0m, Description = "Description2" },
//                 new Product { ProductName = "T-Shirt", Price = 15.75m, Description = "its a t-shirt size 10 cheap and amazing" },
//                 new Product { ProductName = "Ball", Price = 15.75m, Description = "A World class magnificent ball" },
//                 new Product { ProductName = "Shoes", Price = 15.75m, Description = "A pair of shoes" },
//                 new Product { ProductName = "Hat", Price = 15.75m, Description = "A hat" },
//                 new Product { ProductName = "T-Shirt", Price = 15.75m, Description = "its a t-shirt size 10 cheap and amazing" },
//                 new Product { ProductName = "Ball", Price = 15.75m, Description = "A World class magnificent ball" },
//                 new Product { ProductName = "Shoes", Price = 15.75m, Description = "A pair of shoes" },
//                 new Product { ProductName = "Hat", Price = 15.75m, Description = "A hat" },
//                 new Product { ProductName = "T-Shirt", Price = 15.75m, Description = "its a t-shirt size 10 cheap and amazing" },
//             };
//
//             context.Products.AddRange(products);
//             context.SaveChanges();
//
//             // Create Orders
//             var orders = new List<Order>
//             {
//                 new Order
//                 {
//                     CustomerName = "John Doe",
//                     OrderDate = DateTime.Parse("2021-01-01"),
//                     User = users.First(u => u.UserName == "john_doe"),
//                     OrderItems = new List<OrderItem>
//                     {
//                         new OrderItem { Product = products[0], Quantity = 2, Price = products[0].Price },
//                         new OrderItem { Product = products[1], Quantity = 1, Price = products[1].Price }
//                     }
//                 },
//                 new Order
//                 {
//                     CustomerName = "Jane Smith",
//                     OrderDate = DateTime.Parse("2021-02-15"),
//                     User = users.First(u => u.UserName == "jane_sales"),
//                     OrderItems = new List<OrderItem>
//                     {
//                         new OrderItem { Product = products[1], Quantity = 3, Price = products[1].Price },
//                         new OrderItem { Product = products[2], Quantity = 2, Price = products[2].Price }
//                     }
//                 },
//                 new Order
//                 {
//                     CustomerName = "John Doe",
//                     OrderDate = DateTime.Parse("2021-01-01"),
//                     User = users.First(u => u.UserName == "john_doe"),
//                     OrderItems = new List<OrderItem>
//                     {
//                         new OrderItem { Product = products[0], Quantity = 2, Price = products[0].Price },
//                         new OrderItem { Product = products[1], Quantity = 1, Price = products[1].Price }
//                     }
//                 },
//                 new Order
//                 {
//                     CustomerName = "Jane Smith",
//                     OrderDate = DateTime.Parse("2021-02-15"),
//                     User = users.First(u => u.UserName == "jane_sales"),
//                     OrderItems = new List<OrderItem>
//                     {
//                         new OrderItem { Product = products[3], Quantity = 3, Price = products[1].Price },
//                         new OrderItem { Product = products[4], Quantity = 2, Price = products[2].Price }
//                     }
//                 }
//             };
//
//             context.Orders.AddRange(orders);
//             context.SaveChanges();
//         }
//     }
// }

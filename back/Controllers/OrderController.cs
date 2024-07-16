using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CustomerOrderSystem.Models;
using CustomerOrderSystemContext.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CustomerOrderSystem.Services;

namespace CustomerOrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public OrderController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [Authorize(Roles = "Sales")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ToListAsync();
            return Ok(orders);
        }

        [Authorize(Roles = "Customer,Sales")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [Authorize(Roles = "Customer,Sales")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        OrderId = o.Id,
                        Quantity = oi.Quantity,
                        ProductName = oi.ProductName,
                        ProductDescription = oi.ProductDescription,
                        Price = oi.Price,
                        ProductId = oi.ProductId,
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }

        [Authorize(Roles = "Customer,Sales")]
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest request)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            // Console.WriteLine($"UserId: {userId}");
            // Console.WriteLine($"UserName: {userName}");
            // Console.WriteLine($"UserRole: {userRole}");
            var user = await _context.Users
                            .FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return BadRequest("Invalid user");
            }
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
            {
                return BadRequest("Invalid order or order items.");
            }

            var orderItems = new List<OrderItem>();

            foreach (var item in request.OrderItems)
            {
                var existingItem = orderItems.FirstOrDefault(i => i.ProductId == item.ProductId);
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (existingProduct == null)
                {
                    return BadRequest("Product not found.");
                }
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = existingProduct.Id,
                        ProductName = existingProduct.ProductName,
                        ProductDescription = existingProduct.Description,
                        Quantity = item.Quantity,
                        Price = existingProduct.Price
                    });
                }
            }
            var order = new Order
            {
                UserId = user.Id,
                CustomerName = user.UserName,
                OrderDate = request.OrderDate,
                OrderItems = orderItems
            };
            if (request.CustomerName == null || order.CustomerName == null)
            {
                Console.WriteLine($"Order object CustomerName: null sfgdjhgsfdhsfdhjsdafkhj");
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }


        [Authorize(Roles = "Customer,Sales")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponse>> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            var existingOrderItems = await _context.OrderItems.Where(oi => oi.OrderId == id).ToListAsync();
            if (existingOrder == null || existingOrder.OrderItems == null)
            {
                return NotFound("Order not found.");
            }

            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJyYnZlZXIzaDUzNW5uM24zNW55bnk1dW1iYnQiLCJqdGkiOiIzNDI4OTllYS00ZjM5LTRkZDAtODVmZC1kYjYwNGYzMjhjNDAiLCJpYXQiOiIxNzIxMTI3MTYzIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI2ZjQ2YWRmYS1lNzIzLTQ0MmQtYTQ2YS03YzBiYzg3MmFkMTYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoic3RyaW5nQGcuYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkN1c3RvbWVyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9leHBpcmF0aW9uIjoiNy8xNi8yMDI0IDExOjUyOjQzIEFNIiwiZXhwIjoxNzIxMTMwNzYzLCJpc3MiOiJFeGFtcGxlSXNzdWVyIiwiYXVkIjoiRXhhbXBsZUF1ZGllbmNlIn0.Yd62PLpmC1NjI20Ifzo0nN - 8IjYJ3EAbq6Eypv4Jtnw

            existingOrder.CustomerName = request.CustomerName;
            existingOrder.OrderDate = request.OrderDate ?? existingOrder.OrderDate;
            // existingOrder.UserId = request.UserId;
            if (existingOrder != null && existingOrderItems != null)
            {
                var existingOrderItemsDictionary = existingOrderItems.ToDictionary(item => item.ProductId);

                foreach (var item in request.OrderItems)
                {
                    var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    if (existingProduct == null)
                    {
                        return BadRequest("Product not found.");
                    }
                    if (item.ProductId == null || item.Quantity == null)
                    {
                        return BadRequest("ProductId and Quantity must not be null.");
                    }

                    if (existingOrderItemsDictionary.TryGetValue(item.ProductId, out var existingItem))
                    {
                        existingItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        existingOrder.OrderItems.Add(new OrderItem
                        {
                            ProductId = existingProduct.Id,
                            ProductName = existingProduct.ProductName,
                            ProductDescription = existingProduct.Description,
                            Quantity = item.Quantity,
                            Price = existingProduct.Price
                        });
                    }
                }

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                return Ok(existingOrder);
            }
            else
            {
                return NotFound("Order not found.");
            }
        }


        [Authorize(Roles = "Customer,Sales")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderResponse>> DeleteOrder(int id)
        {
            var existingOrder = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (existingOrder == null)
            {
                return NotFound("Order not found.");
            }

            _context.Orders.Remove(existingOrder);
            await _context.SaveChangesAsync();

            return Ok(existingOrder);
        }

        [Authorize(Roles = "Customer,Sales")]
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetOrderProducts(int id)
        {
            // get products details from orderItems
            var products = await _context.OrderItems
                .Where(oi => oi.OrderId == id)
                .Select(oi => new ProductDto
                {
                    Id = oi.ProductId,
                    ProductName = oi.Product.ProductName,
                    Price = oi.Price,
                    Description = oi.Product.Description
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}

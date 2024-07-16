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
						OrderId=o.Id,
                        Quantity = oi.Quantity,
						ProductName=oi.ProductName,
						ProductDescription=oi.ProductDescription,
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

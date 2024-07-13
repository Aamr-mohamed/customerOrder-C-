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

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _context.Orders.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // [Authorize (Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest request)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            // var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            // var id="71a18ec5-b295-4f31-841e-96e80a1c753d";
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
            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJyYnZlZXIzaDUzNW5uM24zNW55bnk1dW1iYnQiLCJqdGkiOiJkOTViMDM1NS0zYmQzLTRiNWEtYTAyMi1iYzgzNTZmNzg3NTEiLCJpYXQiOiIxNzIwOTA5NjY1IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzMmRiYTRlNC03OTNhLTRiNDQtOWJiNS03ZjA1MzYyMGVkZWQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoic3RyaW5nQGcuYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkN1c3RvbWVyIiwiZXhwIjoxNzIwOTEzMjY1LCJpc3MiOiJFeGFtcGxlSXNzdWVyIiwiYXVkIjoiRXhhbXBsZUF1ZGllbmNlIn0.udc7zfm8BkCJt_KU1PkXunXp9lcRXem9QRnOW8jjN_w"

            var orderItems = new List<OrderItem>();

            foreach (var item in request.OrderItems)
            {
                var existingItem = orderItems.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }
            }
            var order = new Order
            {
                // User = user,
                UserId = user.Id,
                CustomerName = request.CustomerName,
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
                    if (item.ProductId == null || item.Quantity == null)
                    {
                        return BadRequest("ProductId and Quantity must not be null.");
                    }

                    if (existingOrderItemsDictionary.TryGetValue(item.ProductId, out var existingItem))
                    {
                        existingItem.Quantity += item.Quantity; // Update quantity
                    }
                    else
                    {
                        existingOrder.OrderItems.Add(new OrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price
                        });
                    }
                }

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                return Ok(request);
            }
            else
            {
                return NotFound("Order not found.");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using CustomerOrderSystem.Models;
using CustomerOrderSystemContext.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CustomerOrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            return Ok(await _context.OrderItems.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        // [Authorize(Roles = "Customer,Sales")]
        [HttpPost("{id}")]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(int id, [FromBody] CreateOrderItemRequest orderItem)
        {
            // 
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            var existingOrderItems = await _context.OrderItems.Where(oi => oi.OrderId == id).ToListAsync();
            var existingOrderItemsDictionary = existingOrderItems.ToDictionary(item => item.ProductId);

            var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);
            if (Product == null)
            {
                return BadRequest("Product not found.");
            }
            if (existingOrderItemsDictionary.TryGetValue(orderItem.ProductId, out var existingItem))
            {
                existingItem.Quantity += orderItem.Quantity;
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = existingProduct.Id,
                    ProductName = existingProduct.ProductName,
                    ProductDescription = existingProduct.Description,
                    Quantity = orderItem.Quantity,
                    Price = existingProduct.Price
                });

            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }



        [Authorize(Roles = "Customer,Sales")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItem(int id, [FromBody] CreateOrderItemRequest orderItem)
        {
            var orderItemToUpdate = await _context.OrderItems.FindAsync(id);
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }
            if (orderItemToUpdate == null)
            {
                return NotFound("OrderItem not found.");
            }

            orderItemToUpdate.ProductId = existingProduct.Id;
            orderItemToUpdate.ProductName = existingProduct.ProductName;
            orderItemToUpdate.Quantity = orderItem.Quantity;
            orderItemToUpdate.Price = existingProduct.Price;
            if (orderItemToUpdate.Quantity == 0)
            {
                _context.OrderItems.Remove(orderItemToUpdate);
                await _context.SaveChangesAsync();
                return Ok("OrderItem removed");
            }

            await _context.SaveChangesAsync();

            return Ok(orderItemToUpdate);
        }

        [Authorize(Roles = "Customer,Sales")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderItemDto>> DeleteOrderItem(int id)
        {
            var orderItemToDelete = await _context.OrderItems.FindAsync(id);
            if (orderItemToDelete == null)
            {
                return NotFound("OrderItem not found.");
            }
            _context.OrderItems.Remove(orderItemToDelete);
            await _context.SaveChangesAsync();

            return Ok(orderItemToDelete);
        }

        [Authorize(Roles = "Customer,Sales")]
        [HttpGet("products")]
        public async Task<IActionResult> getProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

    }


}
